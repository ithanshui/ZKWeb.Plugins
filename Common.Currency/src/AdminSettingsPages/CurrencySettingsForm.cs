﻿using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Currency.src.Config;
using ZKWeb.Plugins.Common.Currency.src.ListItemProviders;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.AdminSettingsPages {
	/// <summary>
	/// 货币设置
	/// </summary>
	[ExportMany]
	public class CurrencySettingsForm : AdminSettingsFormPageBuilder {
		public override string Group { get { return "BaseSettings"; } }
		public override string GroupIconClass { get { return "fa fa-wrench"; } }
		public override string Name { get { return "CurrencySettings"; } }
		public override string IconClass { get { return "fa fa-usd"; } }
		public override string Url { get { return "/admin/settings/currency_settings"; } }
		public override string Privilege { get { return "AdminSettings:CurrencySettings"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 默认货币
			/// </summary>
			[Required]
			[DropdownListField("DefaultCurrency", typeof(CurrencyListItemProvider))]
			public string DefaultCurrency { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<CurrencySettings>();
				DefaultCurrency = settings.DefaultCurrency;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<CurrencySettings>();
				settings.DefaultCurrency = DefaultCurrency;
				configManager.PutData(settings);
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
