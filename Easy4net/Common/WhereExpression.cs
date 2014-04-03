using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy4net.Common
{

    public class WhereExpression
    {

        //左边表达式
        public WhereExpression Left { get; set; }

        //右边表示式
        public WhereExpression Right { get; set; }

        //左边的字段 
        public String LeftField { get; set; }

        //右边的字段
        public object RightField { get; set; }


        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 是否检索全部数据  1=1
        /// </summary>
        internal bool? IsAll { get; set; }

        /// <summary>
        /// 是否为单独的查询
        /// </summary>
        /// <returns></returns>
        public bool IsSingle
        {
            get
            {
                if ((Left as object) == null && (Right as object) == null)
                    return true;
                return
                    false;
            }
        }

        public static bool IsNullOrEmpty(WhereExpression exp)
        {
            if ((exp as object) == null) return true;
            if ((exp.Left as object) == null &&
                (exp.Right as object) == null &&
                (exp.LeftField as object) == null &&
                (exp.RightField as object) == null)
                return true;
            return false;
        }


        public static WhereExpression ALL
        {
            get
            {
                return new WhereExpression()
                {
                    IsAll = true
                };
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        #region 运算符重载



        #region 两个条件表达式的组合

        /// <summary>
        /// And
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static WhereExpression operator &(WhereExpression left, WhereExpression right)
        {
            return null;//WhereExpression.Create(left, right, QueryOperator.And);
        }


        /// <summary>
        /// Or
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static WhereExpression operator |(WhereExpression left, WhereExpression right)
        {
            return null;//WhereExpression.Create(left, right, QueryOperator.Or);
        }
        #endregion

        public WhereExpression Like(string value)
        {
            return null;
        }


        #endregion

        public static bool operator true(WhereExpression exp)
        {
            return false;
        }

        public static bool operator false(WhereExpression exp)
        {
            return false;
        }
    }
}

