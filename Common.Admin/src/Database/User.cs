﻿using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using ZKWeb.Database.UserTypes;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Database {
	/// <summary>
	/// 用户
	/// </summary>
	public class User {
		/// <summary>
		/// 用户Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 用户类型
		/// </summary>
		public virtual UserTypes Type { get; set; }
		/// <summary>
		/// 用户名
		/// </summary>
		public virtual string Username { get; set; }
		/// <summary>
		/// 密码信息，json
		/// </summary>
		public virtual PasswordInfo Password { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 附加数据
		/// </summary>
		public virtual Dictionary<string, object> Items { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 关联的用户角色
		/// </summary>
		public virtual ISet<UserRole> Roles { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public User() {
			Items = new Dictionary<string, object>();
			Roles = new HashSet<UserRole>();
		}

		/// <summary>
		/// 显示用户名
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Username;
		}
	}

	/// <summary>
	/// 用户的扩展函数
	/// </summary>
	public static class UserExtensions {
		/// <summary>
		/// 设置密码
		/// </summary>
		public static void SetPassword(this User user, string password) {
			if (string.IsNullOrEmpty(password)) {
				throw new ArgumentNullException("password");
			}
			user.Password = PasswordInfo.FromPassword(password);
		}

		/// <summary>
		/// 检查密码
		/// </summary>
		public static bool CheckPassword(this User user, string password) {
			if (user.Password == null || string.IsNullOrEmpty(password)) {
				return false;
			}
			return user.Password.Check(password);
		}
	}

	/// <summary>
	/// 用户的数据库结构
	/// </summary>
	[ExportMany]
	public class UserMap : ClassMap<User> {
		/// <summary>
		/// 初始化
		/// </summary>
		public UserMap() {
			Id(u => u.Id);
			Map(u => u.Type).CustomType<int>().Index("Idx_Type");
			Map(u => u.Username).Unique();
			Map(u => u.Password).CustomType<JsonSerializedType<PasswordInfo>>();
			Map(u => u.CreateTime);
			HasManyToMany(u => u.Roles);
			Map(u => u.Items).CustomType<JsonSerializedType<Dictionary<string, object>>>();
			Map(u => u.Deleted);
		}
	}
}
