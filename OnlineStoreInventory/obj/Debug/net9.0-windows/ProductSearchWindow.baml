   M S B A M L   `   `   `  ����  OnlineStoreInventory.    (OnlineStoreInventory.ProductSearchWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$!  Поиск продукта��5   	   $	��500��6    $	��800��6-   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $��* �6   .+�5   
   ��  �� Orientation$ Vertical=�6   $��0��6,   $��0,0,0,10q�69   .+�5      �� $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5      �� $,��&Наименование товара:��6   $	��200��68   $��Center=�6D   �� 5      -   5         ��Name$ SearchTextBox��5      $	��200��61   5      �� 5      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5      �� $'��!Ценновой диапазон��6   $	��200��65   $��Center=�6A   �� 5      -   5        $ MinPriceTextBox��5      $��80��63   $��0,0,5,0q�6>    � ToolTip$ 	Min Price��6O   �� 5      $
��до��6   $��Center=�6&   $��0,0,5,0q�6A   �� 5      -   5        $ MaxPriceTextBox��5      $��80��63   $ 	Max Price��6>   5      �� 5      $ 
Horizontal=�6   $��0,0,0,5q�62   $
��Left=�6C   .+�5      �� $ ��Диапазон веса:��6   $	��200��62   $��Center=�6?   �� 5      -   5        $ MinWeightTextBox��5      $��80��64   $��0,0,5,0q�6?   $ Мин. вес��6P   �� 5      $
��до��6   $��Center=�6&   $��0,0,5,0q�6A   �� 5      -   5        $ MaxWeightTextBox��5      $��80��64   $ Макс. вес��6?   5      �� 5!      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5"      �� $��Категория:��6   $	��200��6.   $��Center=�6:   �� 5#      -   5         ��Name$ CategoryComboBox��5#      $	��200��65    �� DisplayMemberPath$
 Name��5$       �� SelectedValuePath$ Id��5%      5&      �� 5)      -   5        $��
Поиск��5)      $	��100��6%   $��0,5,0,0q�6G   5*      &   System.Windows.Controls.DataGrid  5-   
   -   5          Name$ ResultsDataGrid��5-      $��2��6,   	   AutoGenerateColumns$	 False��69   
   
IsReadOnly$

 True��6U      Columns 5.      0  *System.Windows.Controls.DataGridTextColumn  5/      ,  &System.Windows.Controls.DataGridColumn   Header$- 'Идентификатор товара��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 150 6Z   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6C   �� *Id+6Z     50      $+ %Наименование товара��6%   $ * 6[    6B   �� *Name+6[     51      $ Стоимость��6%   $	 100 6R    68   �� *Price+6R     52      $ 
Масса��6%   $	 100 6O    64   �� *Weight+6O     53      $ Категория��6%   $	 150 6Z    68   �� *Category.Name+6Z   55      56      57      