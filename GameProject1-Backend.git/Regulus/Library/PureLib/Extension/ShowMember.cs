﻿using System;
using System.Collections.Generic;

namespace Regulus.Extension
{
	public static class ShowMember
	{
		public static string ShowMembers<T>(this T obj)
		{
			return ShowMember._ShowMembers(obj, typeof(T), ", ");
		}

		public static string ShowMembers<T>(this T obj, string join_token)
		{
			return ShowMember._ShowMembers(obj, typeof(T),join_token);
		}


		private static string _ShowMembers(object obj,Type obj_type, string join_token)
		{
			var type = obj_type;
			var values = new List<string>();
			if(obj == null)
			{
				values.Add("null");					
			}			
			else if(type.IsValueType )
			{
				values.Add(obj.ToString());					
			}
			else if (type.IsArray)
			{
				
				Array array = (Array)obj;

				foreach (var ele in array)
				{
					var result = _ShowMembers(ele, type.GetElementType(), join_token);
					values.Add(result);
				}				
			}
			else if (type.IsValueType == false)
			{
				var fields = type.GetFields();
				var properties = type.GetProperties();
				var user = obj;

				Array.ForEach(fields, field =>
				{

					var val = field.GetValue(user);
					if (field.FieldType.IsArray)
					{
						var result = _ShowMembers(val, field.FieldType, join_token);
						values.Add(field.Name + " : " + result);
					}
					else
					{

						values.Add(field.Name + " : " + val);
					}

				});

				Array.ForEach(
					properties,
					property =>
					{

						if (property.CanRead)
						{
							var val = property.GetValue(user, null);
							if (property.PropertyType.IsArray)
							{
								var result = _ShowMembers(val, property.PropertyType, join_token);
								values.Add(property.Name + " : " + result);
							}
							else
							{
								values.Add(property.Name + " : " + val);
							}

						}

					});
			}
			

			return string.Join(join_token, values.ToArray());
		}
	}
}
