﻿#pragma checksum "..\..\..\FamilyFinder\ContentTypeSelector.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1BBC550F127FD2D5EC3AD8B961A9E9B1DA78F0CD444B8455E1A015771C364226"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Calculations;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using ZGF.Revit;


namespace ZGF.Revit {
    
    
    /// <summary>
    /// ContentTypeSelector
    /// </summary>
    public partial class ContentTypeSelector : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\FamilyFinder\ContentTypeSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ZGF.Revit.ContentTypeSelector formSpecifyTypes;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\FamilyFinder\ContentTypeSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridMain;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\FamilyFinder\ContentTypeSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelBottomLabel;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\FamilyFinder\ContentTypeSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonOK;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\FamilyFinder\ContentTypeSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonCancel;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\FamilyFinder\ContentTypeSelector.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Infragistics.Windows.DataPresenter.XamDataGrid datagridInfra;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ContentManager;component/familyfinder/contenttypeselector.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\FamilyFinder\ContentTypeSelector.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.formSpecifyTypes = ((ZGF.Revit.ContentTypeSelector)(target));
            return;
            case 2:
            this.gridMain = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.labelBottomLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.buttonOK = ((System.Windows.Controls.Button)(target));
            
            #line 61 "..\..\..\FamilyFinder\ContentTypeSelector.xaml"
            this.buttonOK.Click += new System.Windows.RoutedEventHandler(this.buttonOK_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.buttonCancel = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\..\FamilyFinder\ContentTypeSelector.xaml"
            this.buttonCancel.Click += new System.Windows.RoutedEventHandler(this.buttonCancel_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.datagridInfra = ((Infragistics.Windows.DataPresenter.XamDataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
