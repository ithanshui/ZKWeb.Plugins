﻿using ZKWeb.Plugins.Common.GenericClass.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.GenericClasses {
	/// <summary>
	/// 文章分类
	/// </summary>
	[ExportMany]
	public class ArticleClass : GenericClassBuilder {
		public override string Name { get { return "ArticleClass"; } }
	}
}
