﻿using MifareReaderApp.DataLogic;
using MifareReaderApp.Views.Dialogs;
using MifareReaderApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MifareReaderApp.Models;
using System.Windows.Input;
using System.Windows;
using MifareReaderApp.Stuff.Commands;
using MifareReaderApp.Stuff.Extenstions;
using MifareReaderApp.Stuff;
using System.Windows.Data;

namespace MifareReaderApp.ViewModels
{
    public class OperatorPageViewModel : INotifyPropertyChanged
    {
        #region Properties
        private bool _fieldsIsEnabled;
        public bool FieldsIsEnabled
        {
            get { return _fieldsIsEnabled; }
            set { _fieldsIsEnabled = value; OnPropertyChanged(); }
        }

        private bool _buttonsIsEnabled;
        public bool ButtonsIsEnabled
        {
            get { return _buttonsIsEnabled; }
            set { _buttonsIsEnabled = value; OnPropertyChanged(); }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }

        private User _userPersistence;
        public User UserPersistence
        {
            get { return _userPersistence; }
            set { _userPersistence = value; IsRestoreCommandEnabled = value != null; OnPropertyChanged(); }
        }

        private bool _adminMode;
        public bool AdminMode { get => _adminMode; set { _adminMode = value; OnPropertyChanged(); } }

        public List<Place> AvailablePlaces { get => GetAvailablePlaces(); }

        private bool _isRestoreCommandEnabled = false;
        public bool IsRestoreCommandEnabled { get => _isRestoreCommandEnabled; set { _isRestoreCommandEnabled = value; OnPropertyChanged(); } }
        #endregion

        #region Commands
        public SimpleCommand SaveCommand { get; set; }
        public SimpleCommand RemoveCommand { get; set; }
        public SimpleCommand RestoreCommand { get; set; }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        public OperatorPageViewModel()
        {

            SaveCommand = new SimpleCommand()
            {
                CommandHandler = SaveUser
            };

            RemoveCommand = new SimpleCommand()
            {
                CommandHandler = RemoveUser
            };

            RestoreCommand = new SimpleCommand()
            {
                CommandHandler = RestoreUser
            };

        }

        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void HandleUser(string cardNumber)
        {
            using var userLogic = new UserLogic();
            var result = userLogic.Find(cardNumber);

            if (!result.IsSuccess)
            {
                MessageDialog.ShowDialog(result.Message);
                return;
            }

            if (result.NotFound == true)
            {
                var placeId = AvailablePlaces.FirstOrDefault(x => x.Name == AppConfig.Instance.DefaultPlace)?.Id ?? -1;

                var date = DateTime.Now;
                var newDate = new DateTime(date.Year, date.Month, date.Day, 14, 0, 0);

                User = new User() { Card = cardNumber, Before = newDate, PlaceId = placeId, Staff = false };
            }
            else
                User = result.Entity;

            

            FieldsIsEnabled = true;
            ButtonsIsEnabled = true;
        }

        public void SaveUser(object? entity)
        {
            if (entity is User user)
            {
                if (user.Before < DateTime.Now)
                {
                    MessageDialog.ShowDialog($"Срок действия {user.Before} не может быть раньше текущей даты");
                    return;
                }

                var localizedValues = new (string PropName, string PropValue)[]
                {
                    //user.Name.GetLocalizedPropInfo<User>(x => x.Name),
                    //user.Name2.GetLocalizedPropInfo<User>(x => x.Name2),
                    //user.Surname.GetLocalizedPropInfo<User>(x => x.Surname),
                    //user.Id1.GetLocalizedPropInfo<User>(x => x.Id1),
                    //user.Id2.GetLocalizedPropInfo<User>(x => x.Id2),
                    user.Card.GetLocalizedPropInfo<User>(x => x.Card)
                };

                var validationResult = Validation.ValidateStrings(localizedValues);

                if (!validationResult.IsSuccess)
                {
                    MessageDialog.ShowDialog($"Следующие поля не могут пустыми:\n{string.Join('\n', validationResult.NotValidEntities)}");
                    return;
                }

                using var userLogic = new UserLogic();
                var result = userLogic.Add(user);

                if (!result.IsSuccess)
                {
                    MessageDialog.ShowDialog($"Во время добавления/обновления сущности {nameof(User)} возникла ошибка\n{result.Message}");
                    return;
                }

                if (user != null)
                    UserPersistence = user.Clone();

                MessageDialog.ShowDialog(result.Message);

                if (result.IsSuccess)
                    ResetState();
            }
        }

        public void RemoveUser(object? entity)
        {
            if (entity is User user)
            {
                using var userLogic = new UserLogic();
                var result = userLogic.Delete(user);

                MessageDialog.ShowDialog(result.Message);

                if (result.IsSuccess)
                    ResetState();
            }
        }

        public void RestoreUser(object? entity)
        {
            if (UserPersistence != null && User != null)
            {
                var cardCache = User.Card;
                var idCache = User.Id;
                User = UserPersistence.Clone();
                User.Card = cardCache;
                User.Id = idCache;
            }
        }

        public void ResetState()
        {
            User = null;
            FieldsIsEnabled = false;
            ButtonsIsEnabled = false;
        }

        private List<Place> GetAvailablePlaces()
        {
            using var placeLogic = new HelperEntityLogic<Place>();
            var places = placeLogic.GetAll();

            return places;
        }
    }
}
