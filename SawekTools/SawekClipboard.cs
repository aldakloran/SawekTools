using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace SawekTools {
    public static class SawekClipboard {
        public static IEnumerable<T> PasteFromExcelToClass<T>(int ilosc_wklejanych_kolumn = 1024) where T : class {
            string text = Clipboard.GetText();

            if (string.IsNullOrEmpty(text)) yield break;

            foreach (string wiersz in text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                var cells = wiersz.Split('\t');
                int licznik = 0;

                var new_object = Activator.CreateInstance(typeof(T), true) as T;
                //TypedReference reference = __makeref(NewStruct);
                foreach (PropertyInfo field in typeof(T).GetProperties().OrderBy(x => x.MetadataToken)) {
                    if (!field.CanWrite)
                        continue;

                    if (licznik < cells.Length && licznik < ilosc_wklejanych_kolumn) {
                        string val = cells[licznik];

                        TypeCode typ = Type.GetTypeCode(field.PropertyType);
                        if (field.PropertyType.IsGenericType && field.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                            typ = Type.GetTypeCode(field.PropertyType.GetGenericArguments()[0]);
                        }

                        switch (typ) {
                            case TypeCode.String:
                                field.SetValue(new_object, val);

                                break;
                            case TypeCode.Boolean:
                                if (bool.TryParse(val, out bool res1))
                                    field.SetValue(new_object, res1);

                                break;
                            case TypeCode.Byte:
                                if (byte.TryParse(val, out byte res2))
                                    field.SetValue(new_object, res2);

                                break;
                            case TypeCode.Char:
                                if (char.TryParse(val, out char res3))
                                    field.SetValue(new_object, res3);

                                break;
                            case TypeCode.DateTime:
                                if (DateTime.TryParse(val, out DateTime res4))
                                    field.SetValue(new_object, res4);

                                break;
                            case TypeCode.Decimal:
                                if (decimal.TryParse(val, out decimal res5))
                                    field.SetValue(new_object, res5);

                                break;
                            case TypeCode.Double:
                                if (double.TryParse(val, out double res6))
                                    field.SetValue(new_object, res6);

                                break;
                            case TypeCode.Int16:
                                if (short.TryParse(val, out short res7))
                                    field.SetValue(new_object, res7);

                                break;
                            case TypeCode.Int32:
                                if (int.TryParse(val, out int res8))
                                    field.SetValue(new_object, res8);

                                break;
                            case TypeCode.Int64:
                                if (long.TryParse(val, out long res9))
                                    field.SetValue(new_object, res9);

                                break;
                            case TypeCode.SByte:
                                if (sbyte.TryParse(val, out sbyte res10))
                                    field.SetValue(new_object, res10);

                                break;
                            case TypeCode.Single:
                                if (float.TryParse(val, out float res11))
                                    field.SetValue(new_object, res11);

                                break;
                            case TypeCode.UInt16:
                                if (ushort.TryParse(val, out ushort res12))
                                    field.SetValue(new_object, res12);

                                break;
                            case TypeCode.UInt32:
                                if (uint.TryParse(val, out uint res13))
                                    field.SetValue(new_object, res13);

                                break;
                            case TypeCode.UInt64:
                                if (ulong.TryParse(val, out ulong res14))
                                    field.SetValue(new_object, res14);

                                break;
                            default:
                                Console.WriteLine(@"Inny typ");
                                try {
                                    object new_val = Convert.ChangeType(val, field.PropertyType);
                                    field.SetValue(new_object, new_val);
                                }
                                catch {
                                    // ignored
                                }

                                break;
                        }
                    }

                    licznik++;
                }

                yield return new_object;
            }
        }

        public static IEnumerable<T> PasteFromExcelToStruct<T>(int ilosc_wklejanych_kolumn = 1024) where T : struct {
            string text = Clipboard.GetText();

            if (string.IsNullOrEmpty(text)) yield break;

            foreach (string wiersz in text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                var cells = wiersz.Split('\t');
                int licznik = 0;

                var new_struct = new T();
                TypedReference reference = __makeref(new_struct);
                foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public).OrderBy(x => x.MetadataToken)) {
                    if (licznik < cells.Length && licznik < ilosc_wklejanych_kolumn) {
                        string val = cells[licznik];

                        TypeCode typ = Type.GetTypeCode(field.FieldType);
                        if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                            typ = Type.GetTypeCode(field.FieldType.GetGenericArguments()[0]);
                        }

                        switch (typ) {
                            case TypeCode.String:
                                field.SetValueDirect(reference, val);

                                break;
                            case TypeCode.Boolean:
                                if (bool.TryParse(val, out bool res1))
                                    field.SetValueDirect(reference, res1);

                                break;
                            case TypeCode.Byte:
                                if (byte.TryParse(val, out byte res2))
                                    field.SetValueDirect(reference, res2);

                                break;
                            case TypeCode.Char:
                                if (char.TryParse(val, out char res3))
                                    field.SetValueDirect(reference, res3);

                                break;
                            case TypeCode.DateTime:
                                if (DateTime.TryParse(val, out DateTime res4))
                                    field.SetValueDirect(reference, res4);

                                break;
                            case TypeCode.Decimal:
                                if (decimal.TryParse(val, out decimal res5))
                                    field.SetValueDirect(reference, res5);

                                break;
                            case TypeCode.Double:
                                if (double.TryParse(val, out double res6))
                                    field.SetValueDirect(reference, res6);

                                break;
                            case TypeCode.Int16:
                                if (short.TryParse(val, out short res7))
                                    field.SetValueDirect(reference, res7);

                                break;
                            case TypeCode.Int32:
                                if (int.TryParse(val, out int res8))
                                    field.SetValueDirect(reference, res8);

                                break;
                            case TypeCode.Int64:
                                if (long.TryParse(val, out long res9))
                                    field.SetValueDirect(reference, res9);

                                break;
                            case TypeCode.SByte:
                                if (sbyte.TryParse(val, out sbyte res10))
                                    field.SetValueDirect(reference, res10);

                                break;
                            case TypeCode.Single:
                                if (float.TryParse(val, out float res11))
                                    field.SetValueDirect(reference, res11);

                                break;
                            case TypeCode.UInt16:
                                if (ushort.TryParse(val, out ushort res12))
                                    field.SetValueDirect(reference, res12);

                                break;
                            case TypeCode.UInt32:
                                if (uint.TryParse(val, out uint res13))
                                    field.SetValueDirect(reference, res13);

                                break;
                            case TypeCode.UInt64:
                                if (ulong.TryParse(val, out ulong res14))
                                    field.SetValueDirect(reference, res14);

                                break;
                            default:
                                Console.WriteLine(@"Inny typ");
                                try {
                                    object new_val = Convert.ChangeType(val, field.FieldType);
                                    field.SetValueDirect(reference, new_val);
                                }
                                catch {
                                    // ignored
                                }

                                break;
                        }
                    }

                    licznik++;
                }

                yield return new_struct;
            }
        }

        public static string ListaToString<T>(this ICollection<T> lista) where T : class {
            var sb_ret     = new StringBuilder();
            var sb_header  = new StringBuilder();
            var sb_content = new StringBuilder();

            var all_fields = typeof(T).GetProperties().OrderBy(x => x.MetadataToken);
            foreach (PropertyInfo field in all_fields) {
                sb_header.Append((field.Name + '\t').Replace('\n', ' '));
            }

            sb_ret.AppendLine(sb_header.ToString());

            foreach (T o in lista) {
                foreach (var field in all_fields) {
                    if (!field.CanRead) continue;

                    sb_content.Append((field.GetValue(o)?.ToString() + '\t').Replace('\n', ' '));
                }

                sb_content.Append("\r\n");
            }

            sb_ret.AppendLine(sb_content.ToString());

            return sb_ret.ToString();
        }
    }
}