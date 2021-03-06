﻿using DotLiquid;
using System;
using System.IO;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 延迟引用css文件
	/// 需要配合"render_included_css"标签使用
	/// 这个标签会影响上下文内容，不应该在有缓存的模板模块中使用
	/// </summary>
	/// <example>
	/// {% include_css_later "/static/common.base.css/test.css" %}
	/// {% include_css_later variable %}
	/// </example>
	public class IncludeCssLater : Tag {
		/// <summary>
		/// 添加html到变量中，不重复添加
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var css = (context[RenderIncludedCss.Key] ?? "").ToString();
			var path = (context[Markup.Trim()] ?? "").ToString();
			if (string.IsNullOrEmpty(path)) {
				throw new NullReferenceException("css path can't be empty");
			}
			var html = string.Format(
				"<link href='{0}' rel='stylesheet' type='text/css' />\r\n",
				HttpUtils.HtmlEncode(path));
			if (!css.Contains(html)) {
				css += html;
				context.Environments[0][RenderIncludedCss.Key] = css; // 设置到顶级空间
			}
		}
	}
}
