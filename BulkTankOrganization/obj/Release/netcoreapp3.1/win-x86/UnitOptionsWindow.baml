   M S B A M L   `   `   `  ���� O  KBulkTankOrganization, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:BulkTankOrganizationBulkTankOrganization  ,    &BulkTankOrganization.UnitOptionsWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     4local"clr-namespace:BulkTankOrganization       N� Title$  Unit Options Window��.���  � ColumnDefinitions �� $	��300 ��� $	��280 � � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$��0��$��	Customer:��$��16(�$
��Bold"�$��5,5,0,0q�$
��Left=��� -    ��Name$ lblCustomer��$��0��$��0��$�� ��$��16(�$��0,5,0,0q�$��Right=��� $��0��$��1��$��Serial Number:��$��16(�$
��Bold"�$��5,5,0,0q�$
��Left=��� -   $ 	lblSerial��$��0��$��1��$�� ��$��16(�$��0,5,0,0q�$��Right=��� $��1��$��0��$��Service:��$��16(�$
��Bold"�$��5,5,0,0q�$
��Left=��� -   $ 
lblService��$��1��$��0��$�� ��$��16(�$��0,5,0,0q�$��Right=��� $��1��$��1��$��Status:��$��16(�$
��Bold"�$��5,5,0,0q�$
��Left=��� -   $ 	lblStatus��$��1��$��1��$�� ��$��16(�$��0,5,0,0q�$��Right=��� $��2��$��0��$��Job Number:��$��16(�$
��Bold"�$��5,5,0,0q�$
��Left=��� -   $ lblJobNumber��$��2��$��0��$�� ��$��16(�$��0,5,0,0q�$��Right=��� $��2��$��1��$��Gallons:��$��16(�$
��Bold"�$��5,5,0,0q�$
��Left=��� -   $ 	lblGallon��$��2��$��1��$�� ��$��16(�$��0,5,0,0q�$��Right=��� $��3��$��0��$��2��$��0,15,0,0q��� $��Upload File:��$��4��$��0��$
��Left=�$��Center=�$��0,20,0,0q��� -   $��
cboDocType��$��4��$��0��$	��150��$��25��$��Right=�$��	15,20,0,0q�.@��� .��Certificates�� .��Photos�� .��Misc�� -    ��Name$ 
txtDocName��$��4��$��1��$
��Left=�$	��150��$��30��$��	15,20,0,0q�
��������� -	    ��Name$ 	btnUpload��$��4��$��1��$��Select File��$��Right=�$��0,20,0,0q�$��Center=�$��75��
���"�"�
��������� $��Remove File:��$��5��$��0��$
��Left=�$��Center=�$��0,20,0,0q��� -
   $��	cboFolder��$��5��$��0��$	��150��$��25��$��Right=�$��	15,20,0,0q��� -    ��Name$ cboDocument��$��5��$��1��$
��Left=�$	��150��$��25��$��	15,20,0,0q��� -   $ 	btnRemove��$��5��$��1��$��Remove File��$��Right=�$��0,20,0,0q�$��Center=�$��75��
���  ��
��������� $��Add/Change Job Number:��$��6��$��0��$��Center=�$��Center=�$��0,20,0,0q��� -   $ 	txtJobNum��$��6��$��1��$
��Left=�$��Center=�$	��100��$��
-30,20,0,0q�
��������� -   $ 	btnJobNum��$��6��$��1��$��
Update Job��$��Center=�$��
-15,20,0,0q�$��Center=�$��85��
���"�"�
��������� -   $ btnResetJobNum��$��6��$��1��$��Reset Number��$��Right=�$��0,20,0,0q�$��Center=�$��85��
���  ��
��������� $%��Change Service (Med, Ind, Lzr):��$��7��$��0��$��Center=�$��Center=�$��0,20,0,0q��� -   $ 
cboService��$��7��$��1��$	��150��$��25��$
��Left=�$��	15,20,0,0q�.@��� .��
Industrial�� .��	Medical�� .��Laser�� .��	Unknown�� -   $ 
btnService��$��7��$��1��$��Update Service��$��Right=�$��Center=�$��85��$��0,20,0,0q�
���"�"�
��������� $$��Change Status (Rehab, Refurb):��$��8��$��0��$��Center=�$��Center=�$��0,20,0,0q��� -   $ 	cboStatus��$��8��$��1��$	��150��$��25��$
��Left=�$��	15,20,0,0q�.@��� .��Rehabilitation�� .��Refurbishment�� .��	Unknown�� -   $ 	btnStatus��$��8��$��1��$��Update Status��$��0,20,0,0q�$��Right=�$��Center=�$��85��
���"�"�
��������� $��Tank Comments:��$��9��$��0���� -   $ txtComments��$��10��$��0��$��2��$	��100��$��25,0,5,0q� �� AcceptsReturn$
 True�� �� TextWrapping$
 Wrap=�"	 �� VerticalScrollBarVisibility$
	 Auto=�
 �� 
AcceptsTab$

 True��
��������� -   $ btnViewComment��$��11��$��1��$��View Comments��$��	0,5,150,0q�$��Right=�$��Center=�$��90��
���/����� -   $ 
btnComment��$��11��$��1��$��Submit��$��0,5,15,0q�$��Right=�$��Center=�$��90��
���"�"�
��������� -   $ btnOpenFolder��$��18��$��0��$��View Folder��$��	35,8,15,0q�$
��Left=�$��Center=�$��90��
����  �
��������� -   $ btnCloseFolder��$��18��$��0��$��
Close Unit��$��
175,8,15,0q�$
��Left=�$��Center=�$��90��
���  ��
��������� $��12��$��0��$��2��$��0,6,0,0q��� $��Tank Information��$��13��$��0��$��2��$��Center=�$��Center=�$��20(�$
��Bold"��� $��Serial Number:��$��14��$��0��$��Center=�$��0,5,0,5q��� -   $ txtSerialNumberChange��$��14��$��1��$��Center=�$	��150��$��25��$��0,5,0,5q��� $��National Board Number:��$��15��$��0��$��Center=�$��0,5,0,5q��� -   $ txtNBNumberChange��$��15��$��1��$��Center=�$	��150��$��25��$��0,5,0,5q��� $��Customer Number:��$��16��$��0��$��Center=�$��0,5,0,5q��� -   $ txtCustNumberChange��$��16��$��1��$��Center=�$	��150��$��25��$��0,5,0,5q��� $��Gallons:��$��17��$��0��$��Center=�$��0,5,0,5q��� -   $ txtGallonsChange��$��17��$��1��$��Center=�$	��150��$��25��$��0,5,0,5q��� -   $ btnSubmitChange��$��Submit��$��0��$��18��$��2��$��Right=�$��0,8,80,0q�$��90��
���"�"�
�������