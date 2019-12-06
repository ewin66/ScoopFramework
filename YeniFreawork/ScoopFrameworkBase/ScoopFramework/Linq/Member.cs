﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AnalysisServices.AdomdClient;
using ScoopFramework.Helper;
using Set = ScoopFramework.Helper.Set;

namespace ScoopFramework.Linq
{
    public class Member : Object
    {
        List<object> _values;

        /// <summary>
        /// Returns the set of children of a specified member.
        /// </summary>
        public Set Children => AssembleExtension("Children");
        /// <summary>
        /// Returns the system-generated data member that is associated with a nonleaf member of a dimension.
        /// </summary>
        public Member DataMember => AssembleExtension("DataMember");
        /// <summary>
        /// Returns the hierarchy that contains a specified member, level, or hierarchy.
        /// </summary>
        public Set Dimension => AssembleExtension("Dimension");
        /// <summary>
        /// Returns the first child of a specified member.
        /// </summary>
        public Member FirstChild => AssembleExtension("FirstChild");
        /// <summary>
        /// Returns the first child of the parent of a member.
        /// </summary>
        public Member FirstSibling => AssembleExtension("FirstSibling");
        /// <summary>
        /// Returns the last child of a specified member.
        /// </summary>
        public Member LastChild => AssembleExtension("LastChild");
        /// <summary>
        /// Returns the last child of the parent of a specified member.
        /// </summary>
        public Member LastSibling => AssembleExtension("LastSibling");
        /// <summary>
        /// Returns the hierarchy that contains a specified member or level.
        /// </summary>
        public Set Hierarchy => AssembleExtension("Hierarchy");
        /// <summary>
        /// Returns the name of a dimension, hierarchy, level, or member.
        /// </summary>
        public Member Name => AssembleExtension("Name");
        /// <summary>
        /// Returns the parent of a member.
        /// </summary>
        public Member Parent => AssembleExtension("Parent");
        /// <summary>
        /// Returns the next member in the level that contains a specified member.
        /// </summary>
        public Member NextMember => AssembleExtension("NextMember");
        /// <summary>
        /// Returns the previous member in the level that contains a specified member.
        /// </summary>
        public Member PrevMember => AssembleExtension("PrevMember");
        /// <summary>
        /// Returns the siblings of a specified member, including the member itself.
        /// </summary>
        public Set Siblings => AssembleExtension("Siblings");
        /// <summary>
        /// Returns the unique name of a specified dimension, hierarchy, level, or member.
        /// </summary>
        public Member UniqueName => AssembleExtension("UniqueName");
        /// <summary>
        /// Returns the value of the current member of the Measures dimension that intersects with the current member of the attribute hierarchies in the context of the query.
        /// </summary>
        public Member Value => AssembleExtension("Value");
        /// <summary>
        /// Returns the current member's caption.
        /// </summary>
        public Member Member_Caption => AssembleExtension("Member_Caption");
        /// <summary>
        /// The named of the Member.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Representation of a MDX 'Member'.
        /// </summary>
        /// <param name="objs">The cube objects to assemble into a member.</param>
        public Member(params object[] objs)
        {
            _values = new List<object>();
            foreach (object val in objs)
                _values.Add(val);
        }

        /// <summary>
        /// Representation of a MDX 'Member'.
        /// </summary>
        /// <param name="obj">String representation of a member.</param>
        public Member(string obj)
        {
            _values = new List<object>();
            _values.Add(obj);
        }

        /// <summary>
        /// Mdx 'Item' function. Returns a member from a specified tuple.
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <returns></returns>
        public Member Item(int itemNumber) => $"{assembleMember()}.Item({itemNumber})";
        /// <summary>
        /// MDX 'Lead' function. Returns the member that is a specified number of positions following a specified member along the member's level.
        /// </summary>
        /// <param name="leadCount">A valid numeric expression that specifies a number of member positions.</param>
        /// <returns></returns>
        public Member Lead(int leadCount) => $"{assembleMember()}.Lead({leadCount})";
        /// <summary>
        /// MDX 'Lag' function. Returns the member that is a specified number of positions before a specified member at the member's level.
        /// </summary>
        /// <param name="lagCount">A valid numeric expression that specifies the number of member positions to lag.</param>
        /// <returns></returns>
        public Member Lag(int lagCount) => $"{assembleMember()}.Lag({lagCount})";
        /// <summary>
        /// MDX 'Properties' function. Returns the property from the Member Properties list.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public Member Properties(string property) => $"{assembleMember()}.Properties(\"{property}\")";
        /// <summary>
        /// Default representation of an extention function in MDX.
        /// </summary>
        /// <param name="function">The name or syntax of the function.</param>
        /// <returns></returns>
        public Member Function(string function) => $"{this}.{function}";
        /// <summary>
        /// Returns the MDX syntax of this member.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => assembleMember();
        public static implicit operator string(Member mem) => mem.ToString();
        public static implicit operator Member(string str) => new Member(str);
        public static implicit operator bool(Member mem) => true;
        //public static implicit operator Member(Measure m) { return new Member(m.Tag); }

        public static Member operator &(Member member1, Member member2) => $"({member1}, {member2})";

        public static Member operator &(Member member, Measure measure) => $"({member}, {measure})";

        public static Member operator &(Measure measure, Member member) => $"({measure}, {member})";

        /// <summary>
        /// Factory method to create a new member based on the expression given.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberCreator"></param>
        /// <returns></returns>
        public static Member Create<T>(Func<T, Member> memberCreator) =>
            memberCreator(typeof(T).GetCubeInstance<T>());

        /// <summary>
        /// Represents the ':' (between) operator in MDX syntax.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Set operator |(Member m1, Member m2) => $"({m1} : {m2})";

        public static Set operator |(Member m, Set s) => $"({m} : {s})";

        public static Set operator |(Set s, Member m) => $"({s} : {m})";

        public static Member operator +(Member m1, Member m2) => $"({m1} + {m2})";

        public static Member operator -(Member m1, Member m2) => $"({m1} - {m2})";

        public static Member operator *(Member m1, Member m2) => $"({m1} * {m2})";

        public static Member operator /(Member m1, Member m2) => $"({m1} / {m2})";

        string assembleMember()
        {
            var sb = new StringBuilder();
            if (_values.Count > 1)
                sb.Append("(");

            _values
                .Aggregate((a, b) => $"{a}, {b}")
                .To(sb.Append);

            if (_values.Count > 1)
                sb.Append(")");
            return sb.ToString();
        }

        string AssembleExtension(string str)
        {
            return $"{assembleMember()}.{str}";
        }
    }
}
