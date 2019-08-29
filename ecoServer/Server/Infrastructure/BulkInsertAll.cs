//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Microsoft.EntityFrameworkCore.;
//using System.Data.Entity.Design;
//using System.Data.Entity.Core.EntitClient;
//using System.Data.Entity.Core.Metadata.Edm;
//using System.Data.Entity.Core.Objects;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;

//namespace Abhay
//{
//    partial class MyEntityContext
//    {
//        public void BulkInsertAll<T>(IEnumerable<T> entities)
//        {
//            try
//            {
//                entities = entities.ToArray();
//                string cs = Connection.ConnectionString;
//                var conn = new SqlConnection(cs);
//                conn.Open();

//                Type t = typeof(T);

//                var workspace = this.MetadataWorkspace;

//                Dictionary<string, string> mappings = null;

//                try
//                {
//                    mappings = GetMappings(workspace, this.DefaultContainerName, typeof(T).Name);
//                }
//                catch (Exception)
//                {
//                    // there may be an exception depend case to case. 
//                    // In my case I found some issue initially when my 
//                    // EDMX was corrupted but now this is working fine. 
//                    // This is just to see the exception
//                }

//                // var tableAttribute = (TableAttribute)t.GetCustomAttributes(
//                // typeof(TableAttribute), false).Single();
//                var bulkCopy = new SqlBulkCopy(conn)
//                {
//                    DestinationTableName = t.Name
//                };

//                var properties = t.GetProperties().Where(EventTypeFilter).ToArray();

//                var table = new DataTable();

//                foreach (var property in properties)
//                {
//                    try
//                    {
//                        Type propertyType = property.PropertyType;
//                        if (propertyType.IsGenericType &&
//                            propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
//                        {
//                            propertyType = Nullable.GetUnderlyingType(propertyType);
//                        }

//                        table.Columns.Add(new DataColumn(property.Name, propertyType));

//                        var clrPropertyName = property.Name;

//                        // In case mapping failed to compute, assuming the mapping between Entity and 
//                        // Database Table is same as Generated Entity
//                        var tableColumnName = mappings == null ? property.Name : mappings[property.Name];

//                        bulkCopy.ColumnMappings.Add
//                            (new SqlBulkCopyColumnMapping(clrPropertyName, tableColumnName));
//                    }
//                    catch (Exception)
//                    {
//                        //This is just to see the exception
//                    }
//                }

//                foreach (var entity in entities)
//                {
//                    table.Rows.Add(properties.Select(
//                      property => GetPropertyValue(
//                      property.GetValue(entity, null))).ToArray());
//                }

//                bulkCopy.WriteToServer(table);
//                conn.Close();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        private bool EventTypeFilter(System.Reflection.PropertyInfo p)
//        {
//            //var attribute = Attribute.GetCustomAttribute(p,
//            //    typeof(AssociationAttribute)) as AssociationAttribute;

//            //if (attribute == null) return true;
//            //if (attribute.IsForeignKey == false) return true;

//            //In this case I want to avoid the Reference Type (Custom Class) other than System.String
//            //You can comment the blow case and use the above code

//            if (p.PropertyType.IsClass && !p.PropertyType.Namespace.ToString().Equals("System"))
//                return false;
//            return true;
//        }

//        private object GetPropertyValue(object o)
//        {
//            if (o == null)
//                return DBNull.Value;
//            return o;
//        }

//        private Dictionary<string, string> GetMappings
//             (MetadataWorkspace workspace, string containerName, string entityName)
//        {
//            var mappings = new Dictionary<string, string>();
//            var storageMapping = workspace.GetItem<GlobalItem>(containerName, DataSpace.CSSpace);
//            dynamic entitySetMaps = storageMapping.GetType().InvokeMember(
//                "EntitySetMaps",
//                BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance,
//                null, storageMapping, null);

//            foreach (var entitySetMap in entitySetMaps)
//            {
//                var typeMappings = GetArrayList("TypeMappings", entitySetMap);
//                dynamic typeMapping = typeMappings[0];
//                dynamic types = GetArrayList("Types", typeMapping);

//                if (types[0].Name == entityName)
//                {
//                    var fragments = GetArrayList("MappingFragments", typeMapping);
//                    var fragment = fragments[0];
//                    var properties = GetArrayList("AllProperties", fragment);
//                    foreach (var property in properties)
//                    {
//                        var edmProperty = GetProperty("EdmProperty", property);
//                        var columnProperty = GetProperty("ColumnProperty", property);
//                        mappings.Add(edmProperty.Name, columnProperty.Name);
//                    }
//                }
//            }

//            return mappings;
//        }

//        private dynamic GetProperty(string property, object instance)
//        {
//            var type = instance.GetType();
//            return type.InvokeMember(property, BindingFlags.GetProperty |
//            BindingFlags.NonPublic | BindingFlags.Instance, null, instance, null);
//        }

//        private ArrayList GetArrayList(string property, object instance)
//        {
//            var type = instance.GetType();
//            var objects = (IEnumerable)type.InvokeMember(property,
//            BindingFlags.GetProperty | BindingFlags.NonPublic |
//            BindingFlags.Instance, null, instance, null);
//            var list = new ArrayList();
//            foreach (var o in objects)
//            {
//                list.Add(o);
//            }
//            return list;
//        }
//    }
//}