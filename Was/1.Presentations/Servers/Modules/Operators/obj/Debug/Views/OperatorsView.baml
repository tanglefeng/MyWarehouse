   M S B A M L   `   `   `  ����3  /Kengic.Was.Presentation.Server.Module.OperatorsI    CKengic.Was.Presentation.Server.Module.Operators.Views.OperatorsView   5   
   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   
   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   
   f bTelerik.Windows.Controls, Version=2015.3.1123.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GridView, Version=2015.3.1123.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2015.3.1123.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ctelerik1http://schemas.telerik.com/2008/xaml/presentation    5   
   P LPrism.Wpf, Version=6.1.0.0, Culture=neutral, PublicKeyToken=91a96d2a154366d8$prismhttp://prismlibrary.com/  5   
   !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $��	Operators��5   
   $	��100��6   $��25��5   
    � 	Resources 5	         RadListBoxItemsPanel �� Orientation(  "Telerik.Windows.Controls.RadButton �� 
TargetType �� BasedOn �� Property �� Value��%�   &	        '  i     7 �� 5
   
   .R�5      �� $
��Left=�6   $��Stretch=�5      $ 
Horizontal=�5      5      �� 5   
   "  6   8   .�5      ��  � ��$ 3q�6'   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5   
   �� $��0��6   #��[  5      $��3q�5      .@�5        $��Start��6    ��5       ��  �� Path$ StartCommand���5       �� $ SelectedItems�	 �� ElementName$	 OperatorsGridView��  5      $��Active��6    ��5        �� $ ActiveCommand���5!       �� $ SelectedItems�$	 OperatorsGridView��  5"      $��Deactive��6    ��5#       �� $ DeactiveCommand���5$       �� $ SelectedItems�$	 OperatorsGridView��  5%      $
��Stop��6    ��5&       �� $ StopCommand���5'       �� $ SelectedItems�$	 OperatorsGridView��  5(      $��Restart��6    ��5)       �� $ RestartCommand���5*       �� $ SelectedItems�$	 OperatorsGridView��5+      *  $Telerik.Windows.Controls.RadGridView  5,   
   -   5        $��OperatorsGridView��5-      $��1��5,      ;  5Telerik.Windows.Controls.GridView.GridViewDataControl
   SelectionMode$
 Extended=�5.      $��Stretch=�5/      $��Stretch=�50      *  $Telerik.Windows.Controls.DataControl   ItemsSource 51      �� *	Operators+   Columns 52      3  -Telerik.Windows.Controls.GridViewSelectColumn  53      55      56      57      