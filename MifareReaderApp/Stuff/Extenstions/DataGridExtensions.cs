using MifareReaderApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MifareReaderApp.Stuff.Extenstions
{
    public static class DataGridExtensions
    {
        extension(DataGrid dataGrid)
        {
            public void OnDataGridColumnGenerating(DataGridAutoGeneratingColumnEventArgs e, Type? collectionElementsType = null)
            {
                Type collectionType = dataGrid.ItemsSource.GetType();

                var itemType = collectionElementsType ?? collectionType.GetGenericArguments().SingleOrDefault();

                if (itemType == null && dataGrid.ItemsSource is ListCollectionView view)
                {
                    var prop = view.ItemProperties.FirstOrDefault();

                    if (prop == null)
                        return;

                    var componentTypeProperty = prop.Descriptor.GetType().GetProperty("ComponentType");
                    var componentType = componentTypeProperty?.GetValue(prop.Descriptor) as Type;

                    if (componentType != null)
                        itemType = componentType;
                }

                var propertyType = e.PropertyType.IsGenericType ? e.PropertyType.GenericTypeArguments.First() : e.PropertyType;
                var propertyIsDateTime = MetadataInfo<IAppliedModel>.PropertyIsDateTime(e.PropertyType);

                if (propertyIsDateTime)
                {
                    (e.Column as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy HH:mm";
                }

                var name = MetadataInfo<IAppliedModel>.GetPropertyLocalizedName(itemType, e.PropertyName);

                if (!MetadataInfo<IAppliedModel>.IsVisibleByOverride(itemType, e.PropertyName)
                                && MetadataInfo<IAppliedModel>.PropertyIsVirtual(itemType, e.PropertyName)
                                && !propertyIsDateTime)
                {
                    e.Cancel = true;
                }

                e.Column.Header = name;
                e.Column.CanUserSort = true;
            }
        }
    }
}
