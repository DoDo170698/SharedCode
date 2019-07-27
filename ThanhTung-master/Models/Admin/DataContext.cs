using NPoco;
using QuanLyHoaDon.CodeLogic;
using System;
using System.Collections.Generic;
namespace QuanLyHoaDon.Models
{
    public class DataContext<T> where T : class, new()
    {
        public static Database Instance { get; set; }
        public static string ConnectString = "MVC_ConnectString";
        public DataContext(string connectString = "MVC_ConnectString")
        {
            Instance = new Database(connectString);
            ConnectString = connectString;
        }

        public static List<T> GetListOrDefault(string sql)
        {
            try
            {
                var list = Instance.Fetch<T>(sql);
                return list;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public static T GetById(int id, string dbName = null)
        {
            try
            {
                var typeName = typeof(T).Name;
                if (string.IsNullOrEmpty(typeName))
                    return null;
                var db_Name =SystemConfig.GetValueByKey(dbName);
                var table = string.IsNullOrEmpty(dbName)
                                   ? string.Format("[dbo].[{0}]", typeName)
                                   : string.Format("[{0}].[dbo].[{1}]", db_Name, typeName);
                var sql = string.Format("SELECT * FROM {0} WHERE ID = {1}", table, id);
                var objectT = Instance.SingleOrDefault<T>(sql);
                return objectT;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public static T GetSingleByField(string field, string value, string dbName = null)
        {
            try
            {
                var sql = Sql.Builder
                        .Select("*")
                        .From(GetTableName(dbName))
                        .Where(string.Format("{0} = {1}", field, value));
                return Instance.SingleOrDefault<T>(sql);
            }
            catch (Exception)
            {

                return null;
            }

        }
        public static bool Insert(T _object, string id = "ID", string dbName = null)
        {
            try
            {
                Instance.Insert<T>(GetTableName(dbName), id, _object);
                return Instance.IsNew(_object);
            }
            catch (Exception)
            {
                return false;
            }

        }
        public static bool Update(T _object, string dbName = null)
        {
            try
            {
                var sql = Sql.Builder;
                foreach (var property in _object.GetType().GetProperties())
                {
                    var name = property.Name;
                    var value = Utils.GetExactValue(property.PropertyType, property.GetValue(_object));
                    if (!Equals(property.Name, "ID"))
                    {
                        if (property.PropertyType == typeof(float) || property.PropertyType == typeof(int) || property.PropertyType == typeof(double) ||
                            property.PropertyType == typeof(byte) || property.PropertyType == typeof(long))
                            sql.Append(string.Format("{0} = {1}", property.Name, value));
                        else if (property.PropertyType == typeof(string) || property.PropertyType == typeof(char))
                            sql.Append(string.Format("{0} = N'{1}'", property.Name, value));
                        else if (property.PropertyType == typeof(DateTime))
                            sql.Append(string.Format("{0} = '{1}'", property.Name, value.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                        else if (property.PropertyType == typeof(DateTime?))
                            sql.Append(string.Format("{0} = '{1}'", property.Name, Equals(value, null) ? null : value.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                        else if (property.PropertyType == typeof(bool))
                        {
                            sql.Append(string.Format("{0} = {1}", property.Name, value ? 1 : 0));
                        }
                    }
                }
                sql.Where(string.Format("ID = {0}", Utils.GetPropValue(_object, "ID").ToString()));
                return Instance.Update<T>(sql) > 0 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public static bool Delete(T _object, string dbName = null)
        {
            try
            {
                var id = _object.GetType().GetProperty("ID").GetValue(_object, null);
                return Instance.Delete(id) > 0 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public static bool Delete(int id)
        {
            try
            {
                return Instance.Delete<T>(id) > 0 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool Exists(int id)
        {
            try
            {
                return Instance.Exists<T>(id);
            }
            catch
            {
                return false;
            }
        }
        public static string GetTableName(string dbName = null)
        {
            var typeName = typeof(T).Name;
            if (string.IsNullOrEmpty(typeName))
                return null;
            var db_Name = SystemConfig.GetValueByKey(dbName);
            var table = string.IsNullOrEmpty(dbName)
                               ? string.Format("[dbo].[{0}]", typeName)
                               : string.Format("[{0}].[dbo].[{1}]", db_Name, typeName);
            return table;
        }
        public static bool FieldExist(string field, object value, int idChannel = 0)
        {
            try
            {
               var sql = Sql.Builder
                             .Select("ID").From(GetTableName());
                var typeValue = value.GetType();
                var exactValue = Utils.GetExactValue(typeValue,value);
                if (typeValue == typeof(float) || typeValue == typeof(int) || typeValue == typeof(double) ||
                    typeValue == typeof(byte) || typeValue == typeof(long))
                    sql.Append(string.Format("WHERE {0} = {1}", field, exactValue));
                else if (typeValue == typeof(string) || value.GetType() == typeof(char))
                    sql.Append(string.Format("WHERE {0} = N'{1}'", field, exactValue));
                else if (typeValue == typeof(DateTime))
                    sql.Append(string.Format("WHERE {0} = '{1}'", field, exactValue.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                else if (typeValue == typeof(DateTime?))
                    sql.Append(string.Format("WHERE {0} = '{1}'", field, Equals(exactValue, null) ? null : exactValue.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                else if (typeValue == typeof(bool))
                    sql.Append(string.Format("WHERE {0} = {1}", field, exactValue ? 1 : 0));


                var _object =Equals(Instance,null)
                            ? new Database(ConnectString).FirstOrDefault<T>(sql)
                            : Instance.FirstOrDefault<T>(sql);
                return Equals(_object, null) ? false : true;

            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}