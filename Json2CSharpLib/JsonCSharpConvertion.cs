﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pluralize.NET.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Json2CSharpLib
{
    public static class JsonCSharpConvertion
    {
        public static string Convert(string input)
        {
            var inputLines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var output = new StringBuilder();
            var classNum = 0;
            var anonumousObjects = false;
            for (int i = 0; i < inputLines.Length; i++)
            {

                var newLine = string.Empty;
                var n = inputLines[i].Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Trim().Length > 0).Select(X => X.Trim()).ToList();
                if (inputLines[i].Contains("{"))
                {
                    if (n.Count == 1)
                    {
                        if (anonumousObjects && classNum == 1)
                        {
                            newLine = $"{GetIntent(classNum)}new Object{Environment.NewLine}{GetIntent(classNum)}{{";
                        }
                        else
                        {
                            newLine = $"{GetIntent(classNum)}{{";
                        }
                    }
                    else
                    {
                        var propertyName = n[0].ClearPropertyName();
                        var leftPart = $"{propertyName}";
                        var rightPart = $"new {propertyName.GetClassName(inputLines, i)}";
                        var classIntent = $"{Environment.NewLine}{GetIntent(classNum)}{{";

                        newLine = $"{GetIntent(classNum)}{leftPart} = {rightPart}{classIntent}";
                    }

                    classNum++;
                }
                else if (inputLines[i].Contains("["))
                {
                    var propertyName = n[0].ClearPropertyName();
                    if (propertyName == "[")
                    {
                        propertyName = "Objects";
                        anonumousObjects = true;
                    }
                    var leftPart = $"{propertyName}";
                    var firstLineRightPart = $"new List<{propertyName.GetClassName(inputLines, i).ToSingle()}>";
                    var classIntent = $"{Environment.NewLine}{GetIntent(classNum)}{{";


                    if (n.Count == 1)
                    {
                        newLine = $"{GetIntent(classNum)}{leftPart} = {firstLineRightPart}{classIntent}";
                        classNum++;
                    }
                    else if (inputLines[i].Contains("]"))
                    {
                        newLine = $"{GetIntent(classNum)}{leftPart} = {firstLineRightPart}()";
                    }
                    else
                    {
                        var firstLine = $"{GetIntent(classNum)}{leftPart} = {firstLineRightPart}{classIntent}";
                        classNum++;
                        var secondLineRightPart = $"new {propertyName.GetClassName(inputLines, i).ToSingle()}";
                        var secondLine = $"{GetIntent(classNum)}{secondLineRightPart}";

                        newLine = $"{firstLine}{Environment.NewLine}{secondLine}";
                    }
                }
                else if (inputLines[i].Contains("]"))
                {
                    classNum--;
                    newLine = $"{GetIntent(classNum)}{inputLines[i].Trim().Replace("]", "}")}";
                }
                else if (inputLines[i].Contains("}"))
                {
                    classNum--;
                    newLine = $"{GetIntent(classNum)}{inputLines[i].Trim()}";
                }
                else
                {
                    var leftPart = n[0].Trim('"').FirstCharToUpper();
                    var rightPart = n[1];
                    var comma = rightPart.GetComma();
                    rightPart = rightPart.ClearComma();
                    if (rightPart.IsDate())
                    {
                        rightPart = rightPart.GetDate();
                    }
                    else if (rightPart.IsDecimal())
                    {
                        rightPart = rightPart.GetDecimal();
                    }
                    else if (rightPart.IsGuid())
                    {
                        rightPart = rightPart.GetGuid();
                    }
                    newLine = $"{GetIntent(classNum)}{leftPart} = {rightPart}{comma}";
                }


                output.AppendLine(newLine);
            }

            return output.ToString();
        }

        public static string ConvertCore(string text)
        {
            try
            {
                var jObject = JsonConvert.DeserializeObject<dynamic>(text.Trim());
                return GetString(jObject);
            }
            catch (Exception)
            {
                try
                {
                    var jObject = JsonConvert.DeserializeObject<dynamic>("[" + text.Trim());
                    return GetString(jObject);
                }
                catch (Exception)
                {
                    try
                    {
                        var jObject = JsonConvert.DeserializeObject<dynamic>("{" + text.Trim());
                        return GetString(jObject);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
        }

        private static string GetString(JToken jObject)
        {
            var output = string.Empty;
            switch (jObject.Type)
            {
                case JTokenType.None:
                    break;
                case JTokenType.Object:
                    output = GetStringFromObject(jObject);
                    break;
                case JTokenType.Array:
                    output = GetStringFromArray((JArray)jObject);
                    break;
                case JTokenType.Constructor:
                    break;
                case JTokenType.Property:
                    output = GetStringFromProperty((JProperty)jObject);
                    break;
                case JTokenType.Comment:
                    break;
                case JTokenType.Integer:
                    output = GetStringFromInterger((JValue)jObject);
                    break;
                case JTokenType.Float:
                    output = GetStringFromFloat((JValue)jObject);
                    break;
                case JTokenType.String:
                    output = GetStringFromString((JValue)jObject);
                    break;
                case JTokenType.Boolean:
                    output = GetStringFromBoolean((JValue)jObject);
                    break;
                case JTokenType.Null:
                    output = GetStringFromNull((JValue)jObject);
                    break;
                case JTokenType.Undefined:
                    break;
                case JTokenType.Date:
                    output = GetStringFromDate((JValue)jObject);
                    break;
                case JTokenType.Raw:
                    break;
                case JTokenType.Bytes:
                    break;
                case JTokenType.Guid:
                    break;
                case JTokenType.Uri:
                    break;
                case JTokenType.TimeSpan:
                    break;
                default:
                    break;
            }

            return output;
        }

        private static string GetStringFromDate(JValue jObject)
        {
            return GetIntent(GetClassNum(jObject)) + jObject.Parent.ToString().Split(new string[] { "\":" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim().GetDate();
        }

        private static string GetStringFromBoolean(JValue jObject)
        {
            return GetIntent(GetClassNum(jObject)) + jObject.Value.ToString().ToLower();
        }

        private static string GetStringFromNull(JValue jObject)
        {
            return GetIntent(GetClassNum(jObject)) + "null";
        }

        private static string GetStringFromFloat(JValue jObject)
        {
            return GetIntent(GetClassNum(jObject)) + jObject?.Value.ToString() + "m";
        }

        private static string GetStringFromInterger(JValue jObject)
        {
            return GetIntent(GetClassNum(jObject)) + jObject?.Value.ToString();
        }

        private static string GetStringFromString(JValue jObject)
        {
            var propertyValue = "\"" + jObject.Value.ToString() + "\"";
            if (propertyValue.IsGuid())
            {
                propertyValue = propertyValue.GetGuid();
            }

            return GetIntent(GetClassNum(jObject)) + propertyValue;
        }

        private static string GetStringFromArray(JArray jObject)
        {
            string output;
            var outputStringBuilder = new StringBuilder();
            var classNum = GetClassNum(jObject);
            var propertyName = jObject.Path.Split('.').Last().ToString().ClearPropertyName();
            var className = GetClassName(jObject, propertyName);

            var newLine = $"new List<{className}>";
            outputStringBuilder.Append(newLine);
            if (jObject.Children().Any())
            {
                outputStringBuilder.Append($"{Environment.NewLine}{GetIntent(classNum)}{{{Environment.NewLine}");

                var childrenText = new List<string>();
                foreach (var item in jObject.Children())
                {
                    childrenText.Add(GetString(item));
                }

                outputStringBuilder.Append(string.Join($",{Environment.NewLine}", childrenText));

                newLine = $"{Environment.NewLine}{GetIntent(classNum)}{"}"}";
                outputStringBuilder.Append(newLine);
            }
            else
            {
                outputStringBuilder.Append($"()");
            }

            output = outputStringBuilder.ToString();
            return output;
        }


        private static string GetStringFromProperty(JProperty jObject)
        {
            string output;
            var outputStringBuilder = new StringBuilder();
            var classNum = GetClassNum(jObject);

            var propertyName = jObject.Name.ClearPropertyName();
            var propertyValue = GetString(jObject.Value);


            var newLine = $"{GetIntent(classNum)}{propertyName} = {propertyValue}";
            outputStringBuilder.Append(newLine);

            output = outputStringBuilder.ToString();
            return output;
        }

        private static string GetStringFromObject(JToken jObject)
        {
            string output;
            var outputStringBuilder = new StringBuilder();
            var classNum = GetClassNum(jObject);

            var propertyName = jObject.Path.Split(new string[] { "." }, StringSplitOptions.None)?.Last()?.ClearPropertyName();
            var className = GetClassName(jObject, propertyName); ;

            if (string.IsNullOrWhiteSpace(className))
            {
                className = "Object";
            }

            var newLine = string.Empty;
            if (jObject?.Parent?.Type != JTokenType.Property)
            {
                newLine += $"{GetIntent(classNum)}";
            }

            newLine += $"new {className}{Environment.NewLine}{GetIntent(classNum)}{{{Environment.NewLine}";
            outputStringBuilder.Append(newLine);

            var childrenText = new List<string>();
            foreach (var item in jObject.Children())
            {
                childrenText.Add(GetString(item));
            }

            outputStringBuilder.Append(string.Join($",{Environment.NewLine}", childrenText));

            newLine = $"{Environment.NewLine}{GetIntent(classNum)}{"}"}";
            outputStringBuilder.Append(newLine);


            output = outputStringBuilder.ToString();
            return output;
        }

        private static string GetClassName(JToken jObject, string propertyName)
        {
            if (jObject.Children().Count() > 0 && jObject.Children().First() is JValue)
            {
                return jObject.Children().First().Type.ToString().ToLower();
            }
            else
            {
                if (propertyName.Contains("Audit"))
                {
                    return "AuditData";
                }

                if (jObject.Children().Any() && jObject.Children().First() is JProperty && jObject.Children().Any(x => (x as JProperty)?.Name == "schemeData"))
                {
                    return "Classification";
                }
                else if (jObject.Children().Any() && jObject.Children().First() is JToken && jObject.Children().First().Children().First() is JProperty && jObject.Children().First().Children().Any(x => (x as JProperty)?.Name == "schemeData"))
                {
                    return "Classification";
                }              
                return Singularize(string.IsNullOrEmpty(propertyName) ? "Objects" : propertyName);
            }
        }

        private static string Singularize(string value)
        {
            if (value.EndsWith("List"))
            {
                return value.Substring(0, value.Length - 4);
            }

            if (value.EndsWith("Data"))
            {
                return value;
            }
         

            for (int i = 1; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                {
                    var firstWord = value.Substring(0, i);
                    var lastWord = Singularize(value.Substring(i, value.Length - i));
                    return firstWord + lastWord;
                }
            }

            return new Pluralizer().Singularize(value);
        }

        private static int GetClassNum(JToken jObject)
        {
            var pathEntries = jObject.Path.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            var classNum = pathEntries.Length;
            if (pathEntries.Length > 0)
            {
                classNum += pathEntries.Count(x => x.Contains("["));
            }

            if (!(jObject.Parent is JArray) && jObject is JValue)
            {
                classNum = 0;
            }
            return classNum;
        }

        private static string GetIntent(int classNum)
        {
            return new string('\t', classNum);
        }
    }
}
