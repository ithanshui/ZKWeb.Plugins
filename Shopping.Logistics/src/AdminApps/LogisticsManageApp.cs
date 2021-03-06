﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.AdminApps;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Shopping.Logistics.src.ListItemProviders;
using ZKWeb.Templating;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWebStandard.Extensions;
using Newtonsoft.Json;
using ZKWeb.Plugins.Shopping.Logistics.src.Model;
using ZKWeb.Plugins.Shopping.Logistics.src.FormFieldAttributes;
using ZKWebStandard.Ioc;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Logistics.src.AdminApps {
	/// <summary>
	/// 物流管理
	/// </summary>
	[ExportMany]
	public class LogisticsManageApp : AdminAppBuilder<Database.Logistics> {
		public override string Name { get { return "LogisticsManage"; } }
		public override string Url { get { return "/admin/logistics"; } }
		public override string TileClass { get { return "tile bg-aqua"; } }
		public override string IconClass { get { return "fa fa-truck"; } }
		protected override IAjaxTableCallback<Database.Logistics> GetTableCallback() { return new TableCallback(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 初始化
		/// </summary>
		public LogisticsManageApp() {
			IncludeCss.Add("/static/common.region.css/region-editor.css");
			IncludeCss.Add("/static/shopping.logistics.css/logistics-edit.css");
			IncludeJs.Add("/static/common.region.js/region-editor.min.js");
			IncludeJs.Add("/static/shopping.logistics.js/logistics-edit.min.js");
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<Database.Logistics> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public void OnBuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupForCrudPage<LogisticsManageApp>();
				searchBar.StandardSetupForCrudPage<LogisticsManageApp>("Name/Remark");
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.Logistics> query) {
				// 按回收站
				query = query.FilterByRecycleBin(request);
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Name.Contains(request.Keyword) ||
						q.Remark.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.Logistics> query) {
				query = query.OrderByDescending(q => q.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(AjaxTableSearchRequest request, List<EntityToTableRow<Database.Logistics>> pairs) {
				foreach (var pair in pairs) {
					var owner = pair.Entity.Owner;
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.Name;
					pair.Row["Type"] = new T(pair.Entity.Type);
					pair.Row["Owner"] = owner == null ? null : owner.Username;
					pair.Row["OwnerId"] = owner == null ? null : (long?)owner.Id;
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["LastUpdated"] = pair.Entity.LastUpdated.ToClientTimeString();
					pair.Row["DisplayOrder"] = pair.Entity.DisplayOrder;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public void OnResponse(AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupForCrudPage<LogisticsManageApp>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddHtmlColumn("Name", "30%");
				response.Columns.AddMemberColumn("Type");
				response.Columns.AddEditColumnForCrudPage<UserManageApp>("Owner", "OwnerId");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				response.Columns.AddActionColumn().StandardSetupForCrudPage<LogisticsManageApp>(request);
			}
		}

		/// <summary>
		/// 添加和编辑物流使用的表单
		/// </summary>
		public class Form : TabDataEditFormBuilder<Database.Logistics, Form> {
			/// <summary>
			/// 名称
			/// </summary>
			[Required]
			[StringLength(500, MinimumLength = 1)]
			[TextBoxField("Name", "Please enter name")]
			public string Name { get; set; }
			/// <summary>
			/// 类型
			/// </summary>
			[Required]
			[RadioButtonsField("LogisticsType", typeof(LogisticsTypeListItemProvider))]
			public string Type { get; set; }
			/// <summary>
			/// 所属人
			/// </summary>
			[TextBoxField("Owner", "Owner")]
			public string Owner { get; set; }
			/// <summary>
			/// 显示顺序
			/// </summary>
			[Required]
			[TextBoxField("DisplayOrder", "Order from small to large")]
			public long DisplayOrder { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[TextAreaField("Remark", 5, "Remark")]
			public string Remark { get; set; }
			/// <summary>
			/// 运费规则的提示信息
			/// </summary>
			[HtmlField("PriceRulesAlert", Group = "LogisticsPriceRules")]
			public HtmlString PriceRulesAlert { get; set; }
			/// <summary>
			/// 运费规则
			/// </summary>
			[LogisticsPriceRulesEditor("PriceRules", Group = "LogisticsPriceRules")]
			public List<PriceRule> PriceRules { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(DatabaseContext context, Database.Logistics bindFrom) {
				Name = bindFrom.Name;
				Type = bindFrom.Type ??
					new LogisticsTypeListItemProvider().GetItems().Select(i => i.Value).FirstOrDefault();
				Owner = bindFrom.Owner == null ? null : bindFrom.Owner.Username;
				DisplayOrder = bindFrom.DisplayOrder;
				Remark = bindFrom.Remark;
				var templateManager = Application.Ioc.Resolve<TemplateManager>();
				PriceRulesAlert = new HtmlString(templateManager.RenderTemplate(
					"shopping.logistics/tmpl.price_rules_alert.html", null));
				PriceRules = bindFrom.PriceRules;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(DatabaseContext context, Database.Logistics saveTo) {
				if (saveTo.Id <= 0) {
					saveTo.CreateTime = DateTime.UtcNow;
				}
				saveTo.Name = Name;
				saveTo.Type = Type;
				saveTo.Owner = Owner == null ? null : context.Get<User>(u => u.Username == Owner);
				saveTo.DisplayOrder = DisplayOrder;
				saveTo.Remark = Remark;
				saveTo.PriceRules = PriceRules;
				saveTo.LastUpdated = DateTime.UtcNow;
				return new {
					message = new T("Saved Successfully"),
					script = ScriptStrings.AjaxtableUpdatedAndCloseModal
				};
			}
		}
	}
}
