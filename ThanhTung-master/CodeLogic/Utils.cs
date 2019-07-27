using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QuanLyHoaDon.CodeLogic
{
    public class Utils
    {
        public static Dictionary<int, string> EnumToDictionary<T>()
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("This isn't a enum");
            var dictionary = new Dictionary<int, string>();
            try
            {
                var enumType = typeof(T);
                dictionary = Enum.GetValues(typeof(T))
                                 .Cast<T>()
                                 .ToDictionary(
                                    t => (int)(object)t,
                                    t =>
                                    {
                                        var descriptionString = string.Empty;
                                        try
                                        {
                                            var memberInfo = enumType.GetMember(t.ToString())
                                                                .FirstOrDefault();
                                            var desctiptionAtribute = Equals(memberInfo, null)
                                                                    ? default
                                                                    : memberInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                                            descriptionString = desctiptionAtribute.Description;
                                        }
                                        catch (Exception)
                                        {
                                            descriptionString = "";
                                        }
                                        return descriptionString;
                                    })
                                 ;
            }
            catch (Exception)
            {
                dictionary = new Dictionary<int, string>();
            }

            return dictionary;
        }
        public static string GetDescription<T>(int key)
        {
            var result = string.Empty;
            try
            {
                var enumDictionary = EnumToDictionary<T>();
                var el = enumDictionary.FirstOrDefault(t => t.Key == key);
                return el.Value;
            }
            catch (Exception)
            {
                return "NULL";
            }
        }
        public static object GetPropValue(object _object, string propName)
        {
            try
            {
                return _object.GetType().GetProperty(propName).GetValue(_object, null);
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return null;
            }
        }
        public static dynamic GetExactValue(Type type, object value)
        {
            dynamic result = 0;
            try
            {
                if (type == typeof(string))
                    result = value.ToString();
                else if (type == typeof(char))
                    result = (char)value;
                else if (type == typeof(byte))
                    result = (byte)value;
                else if (type == typeof(int))
                    result = Convert.ToInt32(value);
                else if (type == typeof(long))
                    result = (long)value;
                else if (type == typeof(float))
                    result = (float)value;
                else if (type == typeof(double))
                    result = (double)value;
                else if (type == typeof(bool))
                    result = (bool)value;
                else if (type == typeof(DateTime))
                    result = ConvertToDateTime(value.ToString());
                else if (type == typeof(DateTime?) || type == typeof(DateTime))
                {
                    result = ConvertToDateTimeNull(value.ToString());
                }
                else if (type == typeof(List<string>))
                {
                    result = (List<string>)value;
                }
                else if (type == typeof(int[]))
                {

                    result = value.ToString().Split(',').Select(t => int.Parse(t)).ToArray();
                }
                else if (type == typeof(long[]))
                {
                    result = value.ToString().Split(',').Select(t => long.Parse(t)).ToArray();
                }
                else
                {
                    result = null;
                }
            }
            catch (Exception e)
            {
                var mess = e.Message;
                if (type == typeof(string))
                    result = "";
                else if (type == typeof(char))
                    result = "";
                else if (type == typeof(byte))
                    result = 0;
                else if (type == typeof(int))
                    result = 0;
                else if (type == typeof(long))
                    result = 0;
                else if (type == typeof(float))
                    result = 0;
                else if (type == typeof(double))
                    result = 0;
                else if (type == typeof(bool))
                    result = false;
                else if (type == typeof(DateTime))
                    result = DateTime.Now;
                else if (type == typeof(DateTime?))
                    result = null;
                else if (type == typeof(List<string>))
                {
                    result = null;
                }
                else if (type == typeof(int[]))
                {
                    result = null;
                }
                if (type == typeof(long[]))
                {
                    result = null;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }
        public static string GetDescription<T>(System.Enum value)
        {
            var descriptionString = string.Empty;
            try
            {
                var enumMember = value.GetType()
                                     .GetMember(value.ToString())
                                     .FirstOrDefault();
                var desctiptionAtribute = Equals(enumMember, null)
                                        ? default
                                        : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                descriptionString = Equals(value, null)
                                      ? value.ToString()
                                      : desctiptionAtribute.Description;
            }
            catch (Exception)
            {

                descriptionString = "";
            }
            return descriptionString;
        }
        public static long GetLong(Dictionary<string, string> _data, string name)
        {
            try
            {
                return long.Parse(_data[name]);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static long[] GetLongs(Dictionary<string, string> _data, string name, bool isAjax = false)
        {
            try
            {
                if (isAjax)
                {
                    var dataString = _data[name];
                    if (!dataString.Contains(","))
                    {
                        return new long[] { long.Parse(dataString) };
                    }
                    return JsonConvert.DeserializeObject<long[]>(_data[name]);
                }
                return _data.Where(t => t.Key == name)
                            .Select(t => long.Parse(t.Value)).ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static int GetInt(Dictionary<string, string> _data, string name)
        {
            try
            {
                return int.Parse(_data[name]);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int[] GetInts(Dictionary<string, string> _data, string name, bool isAjax = false)
        {
            try
            {
                if (isAjax)
                {
                    var dataString = _data[name];
                    if (!dataString.Contains(","))
                    {
                        return new int[] { int.Parse(dataString) };
                    }
                    return JsonConvert.DeserializeObject<int[]>(dataString);
                }
                return _data.Where(t => t.Key == name)
                            .Select(t => int.Parse(t.Value)).ToArray();
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return null;
            }
        }
        public static string GetString(Dictionary<string, string> _data, string name)
        {
            try
            {
                return _data[name];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static bool GetBool(Dictionary<string, string> _data, string name)
        {
            try
            {
                var result = _data[name];
                if (Equals(result, "1") || Equals(result.ToLower(), "true"))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return false;
            }
        }
        public static string[] GetStrings(Dictionary<string, string> _data, string name, bool isAjax = false)
        {
            try
            {
                if (isAjax)
                {
                    var dataString = _data[name];
                    if (!dataString.Contains("\","))
                    {
                        return new string[] { dataString };
                    }
                    return JsonConvert.DeserializeObject<string[]>(_data[name]);
                }
                return _data.Where(t => t.Key == name)
                            .Select(t => t.Value).ToArray();
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return null;
            }
        }
        public static string GetStringJoin(string sep, object[] objects)
        {
            try
            {
                return string.Join(sep, objects);
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return "0";
            }
        }
        public static string GetStringJoin(string sep, long[] objects)
        {
            try
            {
                return string.Join(sep, objects);
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return "0";
            }
        }
        public static string GetStringParents(string parents, int parent)
        {
            try
            {
                var parentSplits = parents.ToLongSplit('|');
                var resultSplits = parentSplits.ToList();
                if (!resultSplits.Exists(t => t == parent))
                {
                    resultSplits.Add(parent);
                }
                return string.Join("|", resultSplits);
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return "0";
            }

        }
        public static List<long> GetLongParents(string parents, int parent = 0)
        {
            try
            {
                var parentSplits = parents.ToLongSplit('|');
                var resultSplits = parentSplits.ToList();
                if (!resultSplits.Exists(t => t == parent))
                {
                    resultSplits.Add(parent);
                }
                return resultSplits;
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return new List<long> { 0 };
            }

        }
        public static string DateToString(DateTime? datetime, string regex = "dd-MM-yyyy")
        {
            try
            {
                return datetime.HasValue ? datetime.Value.ToString(regex) : string.Empty;
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return string.Empty;
            }
        }
        public static string DateToString(DateTime datetime, string regex = "dd-MM-yyyy")
        {
            try
            {
                return datetime.ToString(regex);
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return string.Empty;
            }
        }
        public static DateTime ConvertToDateTime(string value)
        {
            try
            {
                return DateTime.ParseExact(value, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return DateTime.MinValue;
            }
        }
        public static DateTime? ConvertToDateTimeNull(string value, string regex = "dd/MM/yyyy HH:mm")
        {
            try
            {
                value = value.Replace("-", "/");
                if (value.Length == "dd/MM/yyyy".Length)
                {
                    regex = "dd/MM/yyyy";
                }
                return DateTime.ParseExact(value, regex, null);
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return null;
            }
        }
        public static bool SaveFile(string filePath, string fileName, string text = "", bool isAppend = false)
        {
            try
            {
                var fullPath = string.Format(@"{0}\{1}", filePath, fileName);
                var streamWriter = new StreamWriter(fullPath, isAppend);
                streamWriter.Write(text);
                streamWriter.Close();
                return true;
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return false;
            }

        }
        public static string UrlFile(string path, string fileName)
        {
            var virtualPath = SystemConfig.GetValueByKey("FileFolder") + @"\" + path;
            var link = "/UpFileNormal/Download?Path=" + virtualPath + "&FileName=" + fileName;
            return link;
        }
        public static int Quater(DateTime date)
        {
            try
            {
                var month = date.Month;
                int quarter = (month + 2) / 3;
                return quarter;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public static int[] GetMonthOfQuater(int quater)
        {
            var months = new int[] { 0 };
            switch (quater)
            {
                case 1:
                    months = new int[] { 1, 2, 3 };
                    break;
                case 2:
                    months = new int[] { 4, 5, 6 };
                    break;
                case 3:
                    months = new int[] { 7, 8, 9 };
                    break;
                case 4:
                    months = new int[] { 10, 11, 12 };
                    break;
                case 5:
                    months = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                    break;
                default:
                    break;
            }
            return months;
        }

        public static string GetStringJoin(string sep, IEnumerable<int> objects)
        {
            try
            {
                return string.Join(sep, objects);
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return "0";
            }
        }
        public static string GetStringJoin(string sep, IEnumerable<string> objects)
        {
            try
            {
                return string.Join(sep, objects);
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return "0";
            }
        }

        public static string GenLinkExport(string url, dynamic objFillter)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(url);
                var types = objFillter.GetType();
                var props = new List<PropertyInfo>(types.GetProperties());
                foreach (var prop in props)
                {
                    try
                    {
                        var propValue = prop.GetValue(objFillter, null);
                        var propType = prop.PropertyType;
                        if (propValue != null)
                        {
                            if (propType == typeof(int))
                                if (propValue > 0 || prop.Name == "IDChannel")
                                    sb.AppendFormat(prop.Name + "={0}&", propValue);
                            if (propType == typeof(DateTime) ? true : propType == typeof(DateTime?))
                            {
                                var valueDate = Utils.DateToString(propValue, "dd-MM-yyyy");
                                if (Equals(valueDate,null))
                                    sb.AppendFormat(prop.Name + "={0}&", valueDate);
                            }
                            if (propType == typeof(string))
                                sb.AppendFormat(prop.Name + "={0}&", propValue);
                            if (propType == typeof(bool))
                                sb.AppendFormat(prop.Name + "={0}&", propValue);
                        }
                        if (propType.IsArray)
                        {
                            var arrays = (Array)prop.GetValue(objFillter, null);
                            for (int i = 0; i < arrays.Length; i++)
                            {
                                sb.AppendFormat(prop.Name + "={0}&", arrays.GetValue(i));
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                url = sb.ToString().TrimEnd('&');
            }
            catch (Exception)
            {
                url = "#";
            }
            return url;
        }
    }
}