//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Xml.Linq;

namespace NotLimited.Framework.Common.Helpers.Xml
{
    public static class XDocumentExtensions
    {
        public static XElement ChildElement(this XContainer node, string name)
        {
            return node.Elements().FirstOrDefault(x => x.Name.LocalName == name);
        }

		public static void ChildElement(this XContainer node, string name, Action<XElement> resultAction, Action notFoundAction = null)
		{
			var child = node.Elements().FirstOrDefault(x => x.Name.LocalName == name);
			if (child != null)
				resultAction(child);
			else
			{
				if (notFoundAction != null)
					notFoundAction();
			}
		}

        public static IEnumerable<XElement> ChildElements(this XContainer node, string name)
        {
            return node.Elements().Where(x => x.Name.LocalName == name);
        }

		public static void ChildElements(this XContainer node, string name, Action<XElement> resultAction)
		{
			foreach (var child in node.Elements().Where(x => x.Name.LocalName == name))
				resultAction(child);
		}

        public static XElement DescendantElement(this XContainer node, params string[] names)
        {
            var cur = node;

            for (int i = 0; i < names.Length; i++)
            {
                cur = node.ChildElement(names[i]);
                if (cur == null)
                    return null;
            }

            return cur as XElement;
        }

        public static IEnumerable<XElement> AncestorElementsWithNames(this XElement node, params string[] names)
        {
            var cur = node;
            while (cur != null)
            {
                if (names.Contains(cur.Name.LocalName))
                    yield return cur;

                cur = cur.Parent;
            }
        }

	    public static void AttributeValue(this XElement element, string name, Action<string> resultAction)
	    {
			var attr = element.Attributes().FirstOrDefault(a => a.Name.LocalName == name);

			if (attr == null)
				return;

		    resultAction(attr.Value);
	    }

        public static string AttributeValue(this XElement element, string name)
        {
            var attr = element.Attributes().FirstOrDefault(a => a.Name.LocalName == name);

            if (attr != null)
                return attr.Value;

            return null;
        }

        public static void AddElement(this XElement element, XElement child)
        {
            element.Add(child);
			child.Name = XName.Get(child.Name.LocalName, element.Name.NamespaceName);
        }

        public static void AddElements(this XElement element, IEnumerable<XElement> children)
        {
            foreach (var child in children)
            {
                element.Add(child);
                element.SetElementsNamespace();
            }
        }

        public static void SetLocalName(this XElement element, string name)
        {
            element.Name = XName.Get(name, element.Name.NamespaceName);
        }

        private static void SetElementsNamespace(this XElement parent)
        {
            foreach (var descendant in parent.Descendants())
                descendant.Name = XName.Get(descendant.Name.LocalName, parent.Name.NamespaceName);
        }
    }
}