// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
    // work around (ItemsControl.GroupStyle doesn't show items in groups in the UIAutomation tree)
    // this class should be public
    internal class ItemsControlWrapperAutomationPeer : ItemsControlAutomationPeer
    {
        public ItemsControlWrapperAutomationPeer(ItemsControl owner)
            : base(owner)
        { }

        override protected ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            return new ItemsControlItemAutomationPeer(item, this);
        }

        override protected string GetClassNameCore()
        {
            return "ItemsControl";
        }

        override protected AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.List;
        }
    }
}

