   M S B A M L   `   `   `  ���� O  KBulkTankOrganization, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:BulkTankOrganizationBulkTankOrganization  '    !BulkTankOrganization.CreateScreen   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     4local"clr-namespace:BulkTankOrganization       N� Title$  Bulk Tank File Naming��.��� $��	0,0,125,0q� � ColumnDefinitions �� $	��175 �$	��175���� $	��230 �$	��240�� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��2��$��/imgs/CIS_Logo.png��$	��100��$��Center=�$��Center=�$	��349��$��10,0,0,0q��� $��Customer��$��1��$��0��$��14(�$��10,10,10,10q��� -   $��txtCustomer��$��1��$��1��$��10,10,-27,10q�$��14(� �� TextAlignment$ Justify=��� $��	Tank Size��$��2��$��0��$��14(�$��10,10,10,10q��� -   $��txtSize��$��2��$��1��$��10,10,-27,10q�$��14(�$ Justify=��� $��Tank Serial Number��$��3��$��0��$��14(�$��10,10,10,10q��� -   $��	txtNumber��$��3��$��1��$��10,10,-27,10q�$��14(�$ Justify=��� $��Tank Customer No.��$��4��$��0��$��14(�$��10,10,10,10q��� -   $��	txtCustNo��$��4��$��1��$��10,10,-27,10q�$��14(�$ Justify=��� $��Tank NB No.��$��5��$��0��$��14(�$��10,10,10,10q��� -   $��txtNBNo��$��5��$��1��$��10,10,-27,10q�$��14(�$ Justify=��� $��Service��$��6��$��0��$��14(�$��10,10,10,10q��� -   $��
cboService��$��1��$��6��$��10,10,-27,10q�.@��� $��
Industrial���� $��Medical���� $��Status��$��7��$��0��$��14(�$��10,10,10,10q��� -   $��	cboStatus��$��1��$��7��$��10,10,-27,10q�.@��� $��Rehab���� $��Refurb���� $��	Comments:��$��8��$��0��$��14(�$��10,10,10,10q��� -    ��Name$ txtComments��$��9��$��0��$��2��$��10,10,-35,10q�$	��130��$
��True��
������� �� AcceptsReturn$
 True�� �� 
AcceptsTab$
 True���� -	    ��Name$ submit��$��Create Folder��$
��Left=�$	��Top=�$��1��$��10��$��	13,14,0,0q�$	��100��$��25��