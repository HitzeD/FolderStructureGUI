   M S B A M L   `   `   `  ����O  KBulkTankOrganization, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:BulkTankOrganizationBulkTankOrganization  5      (    "BulkTankOrganization.TankSelection   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:BulkTankOrganization     5        N� Title$  TankSelection��5	      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5       $
��Auto �6   �� 5!      $
��Auto �6   �� 5"      $
��Auto �6   .+�5%   
   �� $��.\imgs\CIS_Logo.png��6   $��0��6-   $��Center=�6:   �� 5'   
   -   5         ��Name$ 	btnCreate��5'      $��1��6$   $��Center=�61   $	��150��6N   $��Create Folder��6Z   $��	0,20,0,30q�6r   
���/���6�   �� 5)   
   $��2��6   �� 5+   
   $��Customer��6   $��3��6#   $��Center=�60   $��	0,10,0,10q�6M   $��14(�6`   �� 5,   
   -   5         ��Name$ cboCustomer��5,      $��4��6(   $��Center=�65   $	��200��6R    �� Text$ Airgas��6^   $��0,0,0,20q�6�   �� 5.   
   $��5��6   �� 50   
   $��Serial Number:��6   $��6��6)   $��Center=�66   $��0,10,0,0q�6S   �� 51   
   -   5        $ cboSN��51      $��7��6"   $��Center=�6/   $	��200��6L   $��0,15,0,0q�6X    �� 
IsEditable$ False��6j   �� 5>   
   -   5        $ btnFind��5>      $��14��6"   $��Center=�60   $	��150��6M   $��Find Documents��6Y   $��0,30,0,0q�6r   6�   5@      5A      