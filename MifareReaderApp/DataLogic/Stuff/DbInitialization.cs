﻿using MifareReaderApp.Models;
using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Stuff.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.DataLogic.Stuff
{
    public class DbInitialization : BaseLogic
    {
        public void Initialize()
        {
            InitializeHelperEntities<EventsType>("Вход", "Выход", "Запрет прохода");
            InitializeHelperEntities<Point>("КПП1", "КПП2", "КПП3");
            InitializeHelperEntities<PayType>("Нал", "Безнал");
            InitializeHelperEntities<Place>("На территории", "За территорией");
        }

        private void InitializeHelperEntities<T>(params string[] names) where T : class, IHelperEntity, new()
        {
            if (!DbAvailable)
                return;

            foreach (var name in names)
            {
                var set = DbContext.Set<T>();
                var entry = set.SingleOrDefault(x => x.Name == name);
                if (entry == null)
                    set.Add(new T() { Name = name});
            }

            DbContext.SaveChanges();
        }

        public BaseResult EnsureCreated()
        {
            var result = new BaseResult() { Message = "База данных успешно создана" };

            try
            {
                var dbCreated = DbContext.Database.EnsureCreated();
                if (dbCreated == true)
                    Initialize();
                else
                    result.Message = "База данных уже существует";
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = $"Во время создания/инициализации базы данных произошла ошибка\n{e.Message}";
            }

            return result;
        }
    }
}
