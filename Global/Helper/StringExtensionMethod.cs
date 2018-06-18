using HELP.GlobalFile.Global.Encryption;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HELP.GlobalFile.Global.Helper
{
	public static class StringExtensionMethod
	{
		/// <summary>
		/// 1、url里没有key值，在末尾追加
		/// 2、url里有key的值，首先移除之前的key=value，然后在末尾追加
		/// </summary>
		public static string BuildUrl(this string url, string paramKey, object paramValue)
		{
			//假定有url为：http://www.a.com?xx=4&i=

			//改变成：http://www.a.com?&&
			Regex reg = new Regex(string.Format("{0}=[^&]*", paramKey), RegexOptions.IgnoreCase);
			url = reg.Replace(url, "");

			//有?就替换，没有就添加
			if (url.IndexOf("?") == -1)
			{
				url += string.Format("?{0}={1}", paramKey, paramValue);
			}
			else
			{
				//url: http://www.a.com?&&&xx=7
				url += string.Format("&{0}={1}", paramKey, paramValue);
			}

			//用正则去掉多余的&&: http://www.a.com?&xx=7
			Regex reg1 = new Regex("[&]{2,}", RegexOptions.IgnoreCase);
			url = reg1.Replace(url, "&");

			//最后变成： http://www.a.com?xx=7
			url = url.Replace("?&", "?");
			return url;
		}

		public static int ChineseLength(this string source)
		{
			return Encoding.Default.GetBytes(source).Length;
		}

		public static string Encypt(this string source)
		{
			var encryptor = new SHA512Encrypt();
			return encryptor.Encrypt(source);
		}

		public static string RemoveTag(this string source)
		{
			return Regex.Replace(source, @"<[^>]+>", string.Empty);
		}

		public static string RemoveQuote(this string source)
		{
			return Regex.Replace(source, @"\[quote\][\S\s]+\[/quote\]", string.Empty);
		}

		public static string RemoveHtmlSpace(this string source)
		{
			return Regex.Replace(source, @"&nbsp;", string.Empty);
		}

		public static string RemoveInvisible(this string source)
		{
			return Regex.Replace(source,
				"[\\r|\\n|\\t]\\s*",
				string.Empty, RegexOptions.IgnoreCase);
		}

		public static string TrySubstring(this string source, int length)
		{
			if (string.IsNullOrEmpty(source))
			{
				return string.Empty;
			}

			length = source.Length > length ? length : source.Length;
			return source.Substring(0, length);
		}

		/// <summary>
		/// 过滤完所有html标签空格、前后空格、换行等之后从左到右截取length长度字符，并添加“...”
		/// </summary>
		public static string LeftOnlyText(this string source,
			int length, bool withEllipsis = true)
		{
			string beforeTruncation = source
				.RemoveTag()
				.RemoveHtmlSpace()
				.RemoveInvisible()
				.Trim();
			string result = new string(beforeTruncation
				.Take(length).ToArray());
			if (beforeTruncation.Length > length && withEllipsis)
			{
				result += "...";
			}
			return result;
		}

		public static string ParseEnter(this string source)
		{
			if (!string.IsNullOrEmpty(source))
			{
				return source.Replace("\r\n", "<br />")
					.Replace("\r", "<br />")
					.Replace("\n", "<br />");
			}
			return source;
		}

		public static string ParseBr(this string source)
		{
			if (!string.IsNullOrEmpty(source))
			{
				return source.Replace("<br />", "\r\n");
			}
			return source;
		}

		#region Sanitize

		public static string FixTags(this string inputHtml,
			string[] allowedTags, string[] allowedProperties)
		{
			// process each tags in the input string
			string fixedHtml = Regex.Replace(inputHtml,
											 /* "*?"懒惰限定符，保证在一个<>内：e.g <h1 /> */
											 "(<.*?>)",
											 match => fixTag(match, allowedTags, allowedProperties),
											 RegexOptions.IgnoreCase);

			// return the "fixed" input string
			return fixedHtml;
		}

		// remove tag if is not in the list of allowed tags
		private static string fixTag(Match tagMatch,
			string[] allowedTags, string[] allowedProperties)
		{
			string tag = tagMatch.Value;

			// extrag the tag name, such as "a" or "h1"
			Match m = Regex.Match(tag,
								  //(?<tagName>exp))指定“组”的名称
								  @"</?(?<tagName>[^\s/]*)[>\s/]",
								  RegexOptions.IgnoreCase);
			string tagName = m.Groups["tagName"].Value.ToLower();

			// if the tag isn't in the list of allowed tags, it should be removed
			if (Array.IndexOf(allowedTags, tagName) < 0)
			{
				return "";
			}
			else
			{
				// process each properties in the tag string, such as "onclick=", "style="
				return Regex.Replace(tag,
									@"\S+\s*=\s*[" + "\"|" + @" ']\S*\s*[" + "\"|']",
									match => fixProperty(match, allowedProperties),
									RegexOptions.IgnoreCase);
			}
		}

		private static string fixProperty(Match propertyMatch, string[] allowedProperties)
		{
			string property = propertyMatch.Value;

			Match m = Regex.Match(property,
							//(?=exp)也是一种“组”的表示，匹配exp之前的位置
							//这里就是取属性名
							@"(?<prop>\S*)(\s*)(?==)",
							RegexOptions.IgnoreCase);
			string propertyName = m.Groups["prop"].Value.ToLower();

			if (Array.IndexOf(allowedProperties, propertyName) < 0)
			{
				return "";
			}

			return property;
		}

		#endregion Sanitize

		#region NoFollow

		// finds all the links in the input string and processes them using fixLink
		public static string FixLinks(this string input, string[] whitelist)
		{
			// fix the links in the input string
			string fixedInput = Regex.Replace(input,
											  "(<a.*?>)",
											  match => fixLink(match, whitelist),
											  RegexOptions.IgnoreCase);

			// return the "fixed" input string
			return fixedInput;
		}

		// receives a Regex match that contains a link such as
		// <a href="http://too.much.spam/"> and adds ref=nofollow if needed
		private static string fixLink(Match linkMatch, string[] whitelist)
		{
			// retrieve the link from the received Match
			string singleLink = linkMatch.Value;

			// if the link already has rel=nofollow, return it back as it is
			if (Regex.IsMatch(singleLink,
							  @"rel\s*?=\s*?['""]?.*?nofollow.*?['""]?",
							  RegexOptions.IgnoreCase))
			{
				return singleLink;
			}

			// use a named group to extract the URL from the link
			Match m = Regex.Match(singleLink,
								  //TODO：原代码：@"href\s*?=\s*?['""]?(?<url>[^'""]*)['""]?" 感觉也没错呀！
								  @"href\s*=\s*['""]?(?<url>[^'""\s]*)['""]?",
								  RegexOptions.IgnoreCase);
			string url = m.Groups["url"].Value;

			// if URL doesn't contain http:// or https://, assume it's a local link
			if (!url.Contains("http://") && !url.Contains("https://"))
			{
				return singleLink;
			}

			// extract the host name (such as www.cristiandarie.ro) from the URL
			Uri uri = new Uri(url);
			string host = uri.Host.ToLower();

			// if the host is in the whitelist, don't alter it
			if (Array.IndexOf(whitelist, host) >= 0)
			{
				return singleLink;
			}

			// if the URL already has a rel attribute, change its value to nofollow
			string newLink = Regex.Replace(singleLink,
					 @"(?<a>rel\s*=\s*(?<b>['""]?))((?<c>[^'""\s]*|[^'""]*))(?<d>['""]?)?",
					 "${a}nofollow${d}",
					 RegexOptions.IgnoreCase);

			// if the string had a rel attribute that we changed, return the new link
			if (newLink != singleLink)
			{
				return newLink;
			}

			// if we reached this point, we need to add rel=nofollow to our link
			newLink = Regex.Replace(singleLink, "<a", @"<a rel=""nofollow""",
									RegexOptions.IgnoreCase);
			return newLink;
		}

		#endregion NoFollow

		public static bool RealEmpty(this string richHtmlInput)
		{
			if (!string.IsNullOrWhiteSpace(richHtmlInput))
			{
				return string.IsNullOrWhiteSpace(richHtmlInput
					.Replace("\r\n", string.Empty)
					.Replace("\t", string.Empty)
					.RemoveTag()
					.RemoveHtmlSpace());
			}
			return true;
		}
	}
}