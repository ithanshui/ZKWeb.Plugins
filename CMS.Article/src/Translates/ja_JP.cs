﻿using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.CMS.Article.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Article", "文章" },
			{ "ArticleManage", "文章管理" },
			{ "Article management", "文章管理" },
			{ "Title/Summary/Author", "タイトル/概要/作者" },
			{ "ArticleClass", "文章クラス" },
			{ "ArticleTag", "文章タグ" },
			{ "Title", "タイトル" },
			{ "Author", "作者" },
			{ "Summary", "概要" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}