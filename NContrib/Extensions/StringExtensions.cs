﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using NContrib.Culture;

namespace NContrib.Extensions {

    public static class StringExtensions {

        /// <summary>
        /// Alias for Join
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Concat(this IEnumerable<string> strings, string delimiter) {
            return strings.Join(delimiter);
        }

        /// <summary>
        /// Alias for Join with last delimiter
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Concat(this IEnumerable<string> strings, string delimiter, string lastDelimiter) {
            return strings.Join(delimiter, lastDelimiter);
        }

        /// <summary>Indicates that this string contains only the given characters and nothing else</summary>
        /// <param name="input"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool ContainsOnly(this string input, params char[] chars) {
            return input.ToCharArray().All(chars.Contains);
        }

        /// <summary>Gets a digit inside of the string at a specified index.</summary>
        /// <example>Given the string 122240861, the routing number for Schwab, "122240861".DigitAt(6) returns 8</example>
        /// <param name="input"></param>
        /// <param name="index">Character index to look for a digit</param>
        /// <returns><see cref="Int32"/></returns>
        /// <exception cref="ArgumentException">Thrown when the char is not a digit</exception>
        /// <exception cref="IndexOutOfRangeException" />
        public static int DigitAt(this string input, int index) {
            if (input.IsBlank())
                throw new ArgumentNullException(input);

            ushort c = input[index];
            
            if (c < '0' || c > '9')
                throw new ArgumentException(string.Format("The value at index {0} is '{1}' which is not a digit.", index, input[index]));

            return c - 48;
        }

        /// <summary>Returns part of a string starting at the index where the search string was found</summary>
        /// <param name="s"></param>
        /// <param name="searchPattern"></param>
        /// <param name="includeSearchString"></param>
        /// <returns></returns>
        /// <example>"Boston, MA".FromIndexOf(", ", false) => "MA"</example>
        public static string FromIndexOf(this string s, string searchPattern, bool includeSearchString = false) {
            var m = Regex.Match(s, searchPattern);

            if (!m.Success)
                return s;

            var offset = m.Index + (includeSearchString ? 0 : m.Value.Length);
            return s.Substring(offset, s.Length - offset);
        }

        /// <summary>
        /// Compress a string using GZip and the specific encoding. Defaults to UTF-8
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encodingName"> </param>
        /// <returns></returns>
        public static byte[] GZipCompress(this string s, string encodingName = "utf-8")
        {
            var enc = Encoding.GetEncoding(encodingName);
            var buffer = enc.GetBytes(s);
            
            using (var ms = new MemoryStream())
            using (var gz = new GZipStream(ms, CompressionMode.Compress))
            {
                gz.Write(buffer, 0, buffer.Length);
                gz.Close();
                return ms.ToArray();
            }
        }

        /// <summary>Returns part of a string starting at the index where the search char was found</summary>
        /// <param name="s"></param>
        /// <param name="search"></param>
        /// <param name="includeSearchChar"></param>
        /// <returns></returns>
        /// <example>"$123,456.78".FromIndexOf('.', false) => "78"</example>
        public static string FromIndexOf(this string s, char search, bool includeSearchChar = false) {
            var index = s.IndexOf(search);
            var offset = index + (includeSearchChar ? 0 : 1);
            return index == -1 ? s : s.Substring(offset, s.Length - offset);
        }

        public static string Indent(this string s, int count, string indentStr = " ") {
            return Regex.Replace(s, @"(?m)^", indentStr.Repeat(count));
        }

        /// <summary>Indicates if every character in this string is a digit using <see cref="Char.IsDigit(char)"/></summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDigits(this string input) {
            return input.ToCharArray().All(Char.IsDigit);
        }

        /// <summary>
        /// Indicates if this string is an email address
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmailAddress(this string input) {
            return input.IsMatch(RegexLibrary.EmailAddress);
        }

        /// <summary>Indicates if every character in this string is a letter using <see cref="Char.IsLetter(char)"/></summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLetters(this string input) {
            return input.ToCharArray().All(Char.IsLetter);
        }

        /// <summary>Indicates if every character in this string is a letter or a digit using <see cref="Char.IsLetterOrDigit(char)"/> </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLettersOrDigits(this string input) {
            return input.ToCharArray().All(Char.IsLetterOrDigit);
        }

        /// <summary>
        /// Indicates if this string is a mime type
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <example>text/string</example>
        public static bool IsMimeType(this string input) {
            return input.IsMatch(RegexLibrary.MimeType);
        }

        /// <summary>
        /// Indicate if the string is a URL
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUrl(this string input) {
            return input.IsMatch(RegexLibrary.Url);
        }

        /// <summary>Concatenate these strings using the specified delimiter/glue/separator</summary>
        /// <param name="strings"></param>
        /// <param name="delimiter"></param>
        /// <returns>String.Empty if the sequence contains no elements</returns>
        public static string Join(this IEnumerable<string> strings, string delimiter) {

            var arr = strings.ToArray();

            return arr.Length > 0
                ? string.Join(delimiter, arr)
                : null;
        }

        /// <summary>
        /// Concatenate the elements in this sequence using the specified delimiter
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="delimiter"></param>
        /// <returns>string.Empty if the sequence contains no elements</returns>
        public static string Join(this IEnumerable<string> strings, char delimiter) {
            return strings.Join(new String(delimiter, 1));
        }

        /// <summary>
        /// Concatenate these strings using the specified delimiter until the last element,
        /// then use another delimiter. Used for easily making natural language lists such as Apples, Eggs, Bread, and Milk
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="delimiter"></param>
        /// <param name="lastDelimiter"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> strings, string delimiter, string lastDelimiter) {

            var arr = strings.ToArray();

            if (arr.Length == 1)
                return arr.First();

            return arr.Length > 0
                ? string.Join(delimiter, arr, 0, arr.Length - 1) + lastDelimiter + arr.ElementAt(arr.Length - 1)
                : null;
        }

        /// <summary>
        /// Finds all occurrences of the search string and return an array of indexes using current culture case sensitive searching
        /// </summary>
        /// <param name="s"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static int[] IndexesOf(this string s, string search) {
            return s.IndexesOf(search, StringComparison.CurrentCulture);
        }

        /// <summary>Finds all occurrences of the search string and return an array of indexes</summary>
        /// <param name="s"></param>
        /// <param name="search"></param>
        /// <param name="stringComparison">String comparison to use</param>
        /// <returns></returns>
        public static int[] IndexesOf(this string s, string search, StringComparison stringComparison) {
            var index = 0;
            var result = new List<int>();

            while (index < s.Length) {
                index = s.IndexOf(search, index, stringComparison);
                if (index == -1) break;
                result.Add(index);
                index++;
            }

            return result.ToArray();
        }

        /// <summary>Finds all occurrences of the search char and return an array of indexes</summary>
        /// <param name="s"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static int[] IndexesOf(this string s, char search) {
            var index = 0;
            var result = new List<int>();

            while (index < s.Length) {
                index = s.IndexOf(search, index + 1);
                if (index == -1) break;
                result.Add(index);
                index++;
            }

            return result.ToArray();
        }

        /// <summary>Tests to find if the string is null or empty-string (zero-length)</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string input) {
            return String.IsNullOrEmpty(input);
        }

        /// <summary>Tests to find if the string is neither null null nor empty-string (zero-length)</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this string input) {
            return !input.IsEmpty();
        }

        /// <summary>Tests to find if the string is null, empty (zero-length), or contains only whitespace</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsBlank(this string input) {
            return String.IsNullOrEmpty(input) || input.ToCharArray().All(char.IsWhiteSpace);
        }

        /// <summary>Tests to find if the string is not null, not empty (zero-length), and contains more than whitespace</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotBlank(this string input) {
            return !input.IsBlank();
        }

        /// <summary>
        /// Returns the right <paramref name="length"/> characters of the given string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string input, int length) {
            return string.IsNullOrEmpty(input) || input.Length <= length ? input : input.Substring(0, length);
        }

        /// <summary>
        /// Proxy to the <see cref="Regex.IsMatch(string, string)"/> method.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string pattern) {
            return Regex.IsMatch(input, pattern);
        }

        public static string NullIf(this string input, Predicate<string> condition)
        {
            return condition(input) ? null : input;
        }

        public static string NullIfBlank(this string input)
        {
            return input.NullIf(s => s.IsBlank());
        }

        /// <summary>Returns a string Dictionary by parsing a list of delimited key/value pairs</summary>
        /// <param name="input"></param>
        /// <param name="pairSeparator">Regex pattern that separates the pairs of key/values</param>
        /// <param name="keyValueSeparator">Regex pattern that separates the a key from its value. Ex) '='</param>
        /// <param name="comparer">Comparer to use when creating the dictionary.</param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseDictionary(this string input,
            string pairSeparator, string keyValueSeparator, IEqualityComparer<string> comparer) {

            var pattern = string.Format(@"^([^{0}]+)\s*{0}\s*(.*?)$", keyValueSeparator);

            return Regex.Split(input, pairSeparator)
                    .Where(s => s.Trim().Length > 0)
                    .Select(s => {
                        var m = Regex.Match(s, pattern);

                        if (!m.Success)
                            throw new Exception(string.Format("No matches found in string: {0} with regex {1}", s, pattern));

                        return new[] { m.Groups[1].Value, m.Groups[2].Value };
                    })
                    .ToDictionary(x => x[0], x => x[1], comparer);
        }

        /// <summary>Returns a string Dictionary by parsing a list of delimited key/value pairs. Uses default comparer.</summary>
        /// <param name="input"></param>
        /// <param name="pairSeparator">Regex pattern that separates the pairs of key/values</param>
        /// <param name="keyValueSeparator">Regex pattern that separates the a key from its value. Ex) '='</param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseDictionary(this string input, string pairSeparator, string keyValueSeparator) {
            return input.ParseDictionary(pairSeparator, keyValueSeparator, EqualityComparer<string>.Default);
        }

        public static XmlDocument ParseXmlDocument(this string s) {
            var doc = new XmlDocument();
            doc.LoadXml(s);
            return doc;
        }

        /// <summary>
        /// When a match is successful, returns the captured named value.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static string RegexGroupValue(this string input, string pattern, string groupName)
        {
            var m = Regex.Match(input, pattern);

            return !m.Success ? null : m.Groups[groupName].Value;
        }

        /// <summary>
        /// Shortcut to <see cref="Regex.IsMatch(string, string)"/>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool RegexIsMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// Shortcut to <see cref="Regex.Replace(string, string, string)"/>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string RegexReplace(this string input, string pattern, string replacement) {
            return Regex.Replace(input, pattern, replacement);
        }

        /// <summary>Repeats a character, turning it into a string</summary>
        /// <param name="input"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static string Repeat(this char input, int times) {
            return new String(input, times);
        }

        /// <summary>
        /// Repeats a string <paramref name="input"/> <paramref name="times"/>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static string Repeat(this string input, int times) {
            return new StringBuilder().Insert(0, input, times).ToString();
        }

        /// <summary>
        /// Returns the right <paramref name="length"/> characters of the given string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string input, int length) {
            return string.IsNullOrEmpty(input) || input.Length <= length ? input : input.Substring(input.Length - length);
        }

        /// <summary>
        /// Wraps the string as a <see cref="ShorthandString"/>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ShorthandString S(this string s) {
            return new ShorthandString(s);
        }

        /// <summary>
        /// Wraps the string as a <see cref="ShorthandString"/>
        /// </summary>
        /// <param name="s"></param>
        /// <param name="cultureName">Culture name to be used in formatting. Ex) en-GB, sv-SE, ja-JP</param>
        /// <returns></returns>
        public static ShorthandString S(this string s, string cultureName) {
            return new ShorthandString(s, cultureName);
        }

        /// <summary>
        /// Wraps the string as a <see cref="ShorthandString"/>
        /// </summary>
        /// <param name="s"></param>
        /// <param name="formatProvider">Format provider to be used during formatting</param>
        /// <returns></returns>
        public static ShorthandString S(this string s, IFormatProvider formatProvider) {
            return new ShorthandString(s, formatProvider);
        }

        /// <summary>
        /// Splits a string using the standard <see cref="String.Split(char[])"/> method,
        /// then uses ConvertTo&lt;T&gt; on each item to make an array of the proper type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static T[] Split<T>(this string s, params char[] separator) {
            return s.IsBlank() ? new T[0] : s.Split(separator).Select(x => x.ConvertTo<T>()).ToArray();
        }

        /// <summary>
        /// Converts a string to camel case using spaces, dashes, and underscores as breaking points for a new capital letter
        /// </summary>
        /// <param name="s"></param>
        /// <param name="firstCharTransform"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string s, TextTransform firstCharTransform = TextTransform.Lower) {

            // lower-case repeating upper-case letters (title-case, since we're leaving the first capital in-tact)
            s = Regex.Replace(s, @"(?<=\p{Lu})(\p{Lu}+)", m => m.Groups[1].Value.ToLower());

            // replace whitespace, dashes, an underscores with nothing and uppercase the letter after it
            s = Regex.Replace(s, @"[\s\-_]+(\w)(\p{Lu}+)?", m => m.Groups[1].Value.ToUpper());

            // uppercase the letter after a colon. useful in namespaces
            s = Regex.Replace(s, @"(?<=[:])(\p{Ll})?", m => m.Groups[1].Value.ToUpper());

            if (firstCharTransform == TextTransform.Upper)
                return Char.ToUpper(s[0]) + s.Substring(1);

            if (firstCharTransform == TextTransform.Lower)
                return Char.ToLower(s[0]) + s.Substring(1);

            return s;
        }

        /// <summary>
        /// Uses <see cref="ToSnakeCase"/> and just replaces underscores with dashes
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToDashCase(this string s) {
            return s.ToSnakeCase().Replace('_', '-');
        }

        /// <summary>
        /// Returns a string as its <see cref="Encoding.UTF8"/> byte representation, hex encoded
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHex(this string s) {
            return s.ToHex(Encoding.Unicode);
        }

        /// <summary>
        /// Returns a string as its byte representation, hex encoded
        /// </summary>
        /// <param name="s"></param>
        /// <param name="enc"></param>
        /// <returns></returns>
        public static string ToHex(this string s, Encoding enc) {
            return enc.GetBytes(s).ToHex();
        }

        /// <summary>
        /// Pluralizes a word according to Enlgish words optionally dependant on a <paramref name="number"/>
        /// </summary>
        /// <param name="word"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToPlural(this string word, int number = 0) {
            return word.ToPlural(Inflectors.English, number);
        }

        /// <summary>
        /// Pluralizes a word according to the given inflector, optionally dependant on a <paramref name="number"/>
        /// </summary>
        /// <param name="word"></param>
        /// <param name="inflector"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToPlural(this string word, IInflector inflector, int number = 0) {
            return inflector.ToPlural(word, number);
        }

        /// <summary>
        /// Singularizes a word according to English words, optionally dependant on a <paramref name="number"/>
        /// </summary>
        /// <param name="word"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToSingular(this string word, int number = 1) {
            return word.ToSingular(Inflectors.English, number);
        }

        /// <summary>
        /// Singularizes a word according to the given inflector, optionally dependant on a <paramref name="number"/>
        /// </summary>
        /// <param name="word"></param>
        /// <param name="inflector"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToSingular(this string word, IInflector inflector, int number = 1) {
            return inflector.ToSingular(word, number);
        }

        /// <summary>
        /// Returns a string in snake case. Often used for converting object names.
        /// Is smart about repeating capital letters
        /// </summary>
        /// <example>
        /// TransactionID => Transaction_ID
        /// FirstName => First_Name
        /// CPRNumber => CPR_Number
        /// ReferenceIDNumber => Reference_ID_Number
        /// </example>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSnakeCase(this string input) {

            // spaces to underscores
            input = input.Replace(' ', '_');

            // two or more uppercase letters in a row, or a single lower case, followed by upper
            // then lower get separated by an underscore
            // ex) HSBCBank => HSBC_Bank
            // ex) iPhone = i_Phone
            // ex) 24HourATM => 24_Hour_ATM
            // ex) Use24Hour => Use_24_Hour
            input = Regex.Replace(input, @"(?<=\p{Lu}{2,}|\P{Lu})(\P{Ll})(?=\P{Lu})", "_$1");

            // repeating uppercase letters get prefixed with an underscore
            // when the follow a lowercase letter
            // ex) mDNSResponder => m_DNS_Responder
            input = Regex.Replace(input, @"(?<=\p{Ll})(\p{Lu}{2,})", "_$1");

            // repeating underscores replaced with a single underscore
            input = Regex.Replace(input, "_{2,}", "_");

            return input;
        }

        /// <summary>
        /// Converts the given string to title case using the English transformer
        /// </summary>
        /// <param name="input"></param>
        /// <param name="specials"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string input, IEnumerable<string> specials = null) {
            return input.ToTitleCase(TextCaseTrasnformers.English, specials);
        }

        /// <summary>
        /// Converts the input string to title case using the given transformer
        /// </summary>
        /// <param name="input"></param>
        /// <param name="transformer"></param>
        /// <param name="specials"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string input, ITextCaseTransformer transformer, IEnumerable<string> specials = null) {
            return transformer.ToTitleCase(input, specials);
        }

        /// <summary>
        /// If the input string is too long, chops it off and appends the <see cref="trailing"/> string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="lengthLimit">The total maximum string length to return.</param>
        /// <param name="trailing">String to append when truncation is needed</param>
        /// <example>
        /// If you send in 20 characters, have a <see cref="lengthLimit"/> of 10,
        /// <see cref="trailing"/> string of "...", then you'll get 7 characters of the original
        /// string and then the "..." appended to the end.
        /// </example>
        /// <returns></returns>
        public static string Truncate(this string input, int lengthLimit, string trailing = "...")
        {
            return input.Length + trailing.Length <= lengthLimit
                       ? input
                       : input.Left(lengthLimit - trailing.Length) + trailing;
        }

        /// <summary>
        /// Attempts to convert the string to something (T) using the ConvertTo&lt;T&gt; method
        /// If exceptions are encountered, false is returned and the assigner is not called
        /// If no exceptions are found, assigner is called and true is returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">String to parse</param>
        /// <param name="assign">Assignment action delegate. Used to handle the converted result.</param>
        /// <returns><see cref="bool"/></returns>
        /// <example>"1234".TryParse&lt;int&gt;(r => myLocalIntVar = r)</example>
        public static bool TryConvert<T>(this string input, Action<T> assign) {
            return input.TryConvert(s => s.ConvertTo<T>(), assign);
        }

        /// <summary>
        /// Attempts to convert the string to something else using a Converter. If the conversion works
        /// (as in, there are no exceptions thrown), the assignment delegate is called and true is returned.
        /// If any exceptions are found when calling the converter, false is returned and the assigner is not called.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">String to parse</param>
        /// <param name="converter">Conversion delegate. String to T</param>
        /// <param name="assign">Assignment action delegate. Used to handle the converted result.</param>
        /// <returns><see cref="bool"/></returns>
        /// <example>"12".TryParse(s => int.Parse(s), r => myLocalIntVar = r)</example>
        public static bool TryConvert<T>(this string input, Converter<string, T> converter, Action<T> assign) {
            try {
                assign(converter(input));
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Returns a string until the specified search string is found.
        /// The original string is returned if the search string is not found.
        /// The search string is optionally included in the result
        /// </summary>
        /// <param name="s"></param>
        /// <param name="searchPattern">String to search for</param>
        /// <param name="includeSearchString">Include the search string in the result</param>
        /// <returns></returns>
        /// <example>"Sentence one. Sentence Two.".UntilIndexOf(".", true) => "Sentence One."</example>
        /// <example>"Boston, MA".UntilIndexOf("," false) => "Boston"</example>
        public static string UntilIndexOf(this string s, string searchPattern, bool includeSearchString = false) {
            var m = Regex.Match(s, searchPattern);

            if (!m.Success)
                return s;

            var offset = m.Index + (includeSearchString ? m.Value.Length : 0);
            
            return s.Substring(0, offset);
        }

        /// <summary>
        /// URL-encodes the string using UTF8
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlEncode(this string s) {
            return s.UrlEncode(Encoding.UTF8);
        }

        /// <summary>
        /// URL-encodes the string using the given <see cref="Encoding"/>
        /// </summary>
        /// <param name="s"></param>
        /// <param name="enc"></param>
        /// <returns></returns>
        public static string UrlEncode(this string s, Encoding enc) {
            return HttpUtility.UrlEncode(s, enc);
        }

        /// <summary>
        /// Returns a string until the specified search char is found.
        /// The original string is returned if the search char is not found.
        /// The search string is optionally included in the result
        /// </summary>
        /// <param name="s"></param>
        /// <param name="search">String to search for</param>
        /// <param name="includeSearchString">Include the search string in the result</param>
        /// <returns></returns>
        /// <example>"Sentence one. Sentence Two.".UntilIndexOf('.', true) => "Sentence One."</example>
        public static string UntilIndexOf(this string s, char search, bool includeSearchString = false) {
            var index = s.IndexOf(search);
            return index <= 0 ? s : s.Substring(0, index + (includeSearchString ? 1 : 0));
        }

        /// <summary>
        /// Returns an array of words in the given string.
        /// Splits the given string using the regex \W (non-word) element. This is [^a-zA-Z_0-9]
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] Words(this string s) {
            return Regex.Split(s, @"(?s)\W+").Where(m => m.IsNotBlank()).ToArray();
        }

        /// <summary>Returns an array of whitespace-separated elements in the given string. Similar to qw() in Perl</summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] W(this string s) {
            return Regex.Split(s, @"(?s)\s+");
        }

        /// <summary>
        /// Performs text wrapping on a string
        /// </summary>
        /// <param name="s">String to wrap</param>
        /// <param name="maxLineWidth">Maximum size of the lines</param>
        /// <param name="method">Word-breaking method</param>
        /// <param name="breaker">String used to create breaks. Environment.NewLine is a good choice.</param>
        /// <param name="hardBreaker">String used during hard breaks, such as a dash or Environment.NewLine</param>
        /// <returns></returns>
        public static string Wrap(this string s, int maxLineWidth,
            TextWrapMethod method = TextWrapMethod.HardBreakWhenNecessary,
            string breaker = "\r\n",
            string hardBreaker = "") {

            if (maxLineWidth <= 0)
                throw new ArgumentException("Max Width must be greater than zero", "maxLineWidth");

            if (s.Length <= maxLineWidth)
                return s;

            var sb = new StringBuilder();
            var stringPosition = 0;
            var linePosition = 0;
            char[] breakingChars = { ' ', '-', '\n', '\r' };

            while (stringPosition <= s.Length) {
                var nextBreakingPoint = s.IndexOfAny(breakingChars, stringPosition);

                if (nextBreakingPoint <= 0) {
                    var remaining = s.Length - stringPosition;

                    if (remaining > maxLineWidth) {
                        nextBreakingPoint = stringPosition + maxLineWidth;
                    }
                    else {
                        sb.Append(s.Substring(stringPosition));
                        break;
                    }
                }

                var wordSize = nextBreakingPoint - stringPosition + 1;

                linePosition += wordSize;

                if (linePosition >= maxLineWidth) {

                    if (method == TextWrapMethod.HardBreakAlways || wordSize > maxLineWidth) {
                        var segmentSize = maxLineWidth - (linePosition - wordSize) - hardBreaker.Length;

                        if (segmentSize > 0) {
                            sb.Append(s.Substring(stringPosition, segmentSize) + hardBreaker);
                            stringPosition += segmentSize;
                        }
                    }

                    sb.Append(breaker);
                    linePosition = 0;
                }
                else {
                    sb.Append(s.Substring(stringPosition, wordSize));
                    stringPosition += wordSize;
                }

            }

            return sb.ToString();
        }
    }
}
