﻿using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.UserContact.src.DataCallbacks {
	/// <summary>
	/// 在用户删除前删除关联的联系信息
	/// 非常遗憾，NHibernate不支持ManyToOne的Cascade操作（已实测）
	/// 所以只能使用数据库回调来实现
	/// </summary>
	[ExportMany]
	public class RemoveContactBeforeUserDelete : IDataDeleteCallback<User> {
		public void AfterDelete(DatabaseContext context, User data) { }

		public void BeforeDelete(DatabaseContext context, User data) {
			context.DeleteWhere<Database.UserContact>(c => c.User.Id == data.Id);
		}
	}
}
