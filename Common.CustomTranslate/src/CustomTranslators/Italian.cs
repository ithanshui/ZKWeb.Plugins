﻿using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 意大利语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Italian : CustomTranslator {
		public override string Name { get { return "it-IT"; } }
	}
}
