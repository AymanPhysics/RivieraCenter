   M S B A M L   `   `   `  ????	  OMEGA     WpfApplication1.Services_G   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .??5      ?    Bg#???   6   .+?5	   
   ? $??12,12,12,104q?6   ??5
      ?? 5        ?? EndPoint$  0.5,1&?6&    ?? 
StartPoint$ 0.5,0&?67   .??5      ?  ? Color$ White??6#    ? Offset$ 0Q?61   ? 5      $ 	#FF6DD9FF??6#   $ 1Q?65   5      .+?5      ?? -   5        $??txtID??5   Q   $??28??6   $
??Left=?6"   $??
417,14,0,0q?6=   $	??Top=?6^   $	??120??6v    ?? 	MaxLength$ 10??6?   $??0??6?    ?? AutoWordSelection . .??6?   ??5      ?? 5      $  0.5,1&?6*   $ 0.5,0&?6;   .??5      ? $ White??6'   $ 0Q?65   ? 5      $ 	#37FFFF00??6'   $ 1Q?69   5      .??5   &   5      ?? 5      -   5        $??
lblDayDate??5   ?   ???	#FF078200$??1q?6,   $??التاريخ??6@   ???	#FF078200$??28??6i   $
??Left=?6u   $??Center=?6?   $??
291,44,0,0q?6?    ? UseLayoutRounding . $	??Top=?6?   $	??120??6  (  "System.Windows.Controls.DatePicker  5      -   5        $??DayDate??5   n      FirstDayOfWeek$ Saturday=?6   $??28??63   $
??Left=?6?   $??
417,44,0,0q?6Z   $??1??6}   $	??Top=?6?   $	??129??6?   ?? 5      -   5        $??lblNotes??5   ?   ???	#FF078200$??1q?6,   $??ملاحظات??6@   ???	#FF078200$??28??6i   $
??Left=?6u   $??Center=?6?   $??289,437,0,11q?6?    . $??Bottom=?6?   $	??120??6  ?? 5      -   5        $??Notes??5   b    . $??28??6/   $??417,437,0,11q?6;   $
 1000??6Q   $??12??6o   $??Bottom=?6}   $	??385??6?   $
??Left=?6?   .??6?   ??5      ?? 5      $  0.5,1&?6*   $ 0.5,0&?6;   .??5      ? $ White??6'   $ 0Q?65   ? 5      $ 	#37FFFF00??6'   $ 1Q?69   5       .??5!   &   5"      ?? 5#      -   5        $??lblId??5#   ?   ???	#FF078200$??1q?6,   $!??مسلسل الفاتورة??6@   ???	#FF078200$??28??6p   $
??Left=?6|   $??Center=?6?   $??
291,14,0,0q?6?    . $	??Top=?6?   $	??120??6  ?? 5$      -   5        $??Canceled??5$   c   $??ملغى??6   $??28??6&   $
??Left=?62   $??292,374,0,74q?6M   $??2??6s   $??Bottom=?6?   $	??109??6?   ??.  6?   ?? 5%      -   5         ??Name$ lblSerialId??5%   ?   ???	#FF078200$??1q?6,   $??رقم الإيصال??6@   ???	#FF078200$??28??6m   $
??Left=?6y   $??Center=?6?   $??
630,14,0,0q?6?    . $	??Top=?6?   $	??120??6  ?? 5&      -	   5        	 ??Name$	 SerialId??5&   x    . $??28??6/   $
??Left=?6;   $??
758,14,0,0q?6V   $ 5??6j   $??2??6?   $	??Top=?6?   $	??118??6?   {?.  .??6?   ??5'      ?? 5(      $  0.5,1&?6*   $ 0.5,0&?6;   .??5)      ? $ White??6'   $ 0Q?65   ? 5*      $ 	#37FFFF00??6'   $ 1Q?69   5+      .??5,   &   5-      ?? 5.      -
   5        $ lblLastEntry??5.   ?   ???	#FF078200$??1q?6,   $??آخر مسلسل??6@   ???	#FF078200$??28??6k   $??Right=?6w   $??Center=?6?   $??
0,0,136,45q?6?    . $??Bottom=?6?   $	??120??6  ?? 5/      -   5        $	 	LastEntry??5/   x    . $??28??6/   $??Right=?6;   $??	0,0,10,45q?6W   $ 5??6j   $??2??6?   $??Bottom=?6?   $	??118??6?   {?.  .??6?   ??50      ?? 51      $  0.5,1&?6*   $ 0.5,0&?6;   .??52      ? $ White??6'   $ 0Q?65   ? 53      $ 	#37FFFF00??6'   $ 1Q?69   54      .??55   &   56      ?? 57      -   5        $ lblLastSerialId??57   ?   ???	#FF078200$??1q?6,   $??آخر رقم إيصال??6@   ???	#FF078200$??28??6o   $??Right=?6{   $??Center=?6?   $??
0,0,136,11q?6?    . $??Bottom=?6  $	??120??6  ?? 58      -   5        $	 LastSerialId??58   x    . $??28??6/   $??Right=?6;   $??	0,0,10,11q?6W   $ 5??6j   $??2??6?   $??Bottom=?6?   $	??118??6?   {?.  .??6?   ??59      ?? 5:      $  0.5,1&?6*   $ 0.5,0&?6;   .??5;      ? $ White??6'   $ 0Q?65   ? 5<      $ 	#37FFFF00??6'   $ 1Q?69   5=      .??5>   &   5?      ?? 5@      -   5        $	 CaseName??5@   ?    . $??28??6/   $
??Left=?6;   
 ?? 
IsReadOnly
 . ??.  $??
543,77,0,0q?6z   $	 100??6?   $??1??6?   $	??Top=?6?   $	??284??6?   .??6?   ??5A      ?? 5B      $  0.5,1&?6*   $ 0.5,0&?6;   .??5C      ? $ White??6'   $ 0Q?65   ? 5D      $ 	#37FFFF00??6'   $ 1Q?69   5E      .??5F   &   5G      ?? 5H      -   5        $ 	lblCaseId??5H   ?   ???	#FF078200$??1q?6,   $??Patient??6@   ???	#FF078200$??28??6i   $
??Left=?6u   $??Center=?6?   $??
291,77,0,0q?6?    . $	??Top=?6?   $	??120??6  ?? 5I      -   5        $	 CaseId??5I   y    . $??28??6/   $
??Left=?6;   $??
417,78,0,0q?6V   $ 10??6j   $??5??6?   $	??Top=?6?   $	??118??6?   .??6?   ??5J      ?? 5K      $  0.5,1&?6*   $ 0.5,0&?6;   .??5L      ? $ White??6'   $ 0Q?65   ? 5M      $ 	#37FFFF00??6'   $ 1Q?69   5N      .??5O   &   5P      ?? 5Q      -   5         ??Name$ TabControl1??5Q   +   $??6,14,0,0q?6    . $	??223??6Y   $	??Top=?6f   $
??Left=?6~   $	??273??6?   .@?5R      ?? -   5         ??Name$ TabItemTables??5R   ?   $?? ??6   $
??Left=?6$   $	??Top=?6V   .??5S      ? ???	#B4000000$??Stretch=?63   $??Stretch=?6Q   .+?5T      ?? -   5         ??Name$ ScrollViewer1??5T   '   $??Disabled=?6>   .??5U      ? -   5        $	??WR1??5U   (    J? SnapsToDevicePixels . ??5V   "   ?? 5W   &   $  0.5,1&?6:   $ 0.5,0&?6K   .??5X   *   ? $ White??67   $ 0Q?6E   ? 5Y   *   $ 	#FF6DD9FF??67   $ 1Q?6I   5Z   '   5\   #   5^      5_      5`      5a      ?? 5b      -   5        $ TabControl2??5b   -   $??
6,246,0,11q?6    . $
??Left=?6[   $	??273??6v   .@?5c      ?? -   5        $ TabItem1??5c   ?   $?? ??6   $
??Left=?6$   $	??Top=?6Q   .??5d      ? ???	#B4000000$??Stretch=?63   $??Stretch=?6Q   .+?5e      ?? -   5        $ ScrollViewer2??5e   '   $??Disabled=?6>   .??5f      ? -   5        $	??WR2??5f   (    . ??5g   "   ?? 5h   &   $  0.5,1&?6:   $ 0.5,0&?6K   .??5i   *   ? $ White??67   $ 0Q?6E   ? 5j   *   $ 	#FF6DD9FF??67   $ 1Q?6I   5k   '   5m      5n      5o      5p      5q        5r      -   5          Name$ RemainingDate??5r   q   $ Saturday=?6   $??28??63   $
??Left=?6?   $??417,345,0,103q?6Z   $??12??6?   $??Bottom=?6?   $	??130??6?   ?? 5s      -   5         ??Name$
 Done??5s   W   $??Done Remaining??6   $??28??60   $
??Left=?6<   $??14??6e   $??Bottom=?6s   $	??109??6?   $??292,345,0,103q?6?   ??.  6?     5t      -   5        $ CanceledDate??5t   p   $ Saturday=?6   $??28??63   $
??Left=?6?   $??417,374,0,74q?6Z   $??12??6?   $??Bottom=?6?   $	??130??6?   ?? 5u      -   5         ??Name$ EmpIdRemaining??5u   |   $??28??6   $
??Left=?6#   $??886,44,-6,0q?6g   $??1??6?   $	??Top=?6?   $	??188??6?    ?? 
IsEditable . {?.  $ ?? IsSynchronizedWithCurrentItem 6>   Z? ??5v      ?? 5w      $  0.5,1&?6*   $ 0.5,0&?6;   .??5x      ? $ White??6'   $ 0Q?65   ? 5y      $ 	#37FFFF00??6'   $ 1Q?69   5z      5|      ?? 5}      -   5        $ EmpIdCanceled??5}   }   $??28??6   $
??Left=?6#   $??551,374,0,74q?6g   $??1??6?   $??Bottom=?6?   $	??188??6?    . {?.   6>   Z? ??5~      ?? 5      $  0.5,1&?6*   $ 0.5,0&?6;   .??5?      ? $ White??6'   $ 0Q?65   ? 5?      $ 	#37FFFF00??6'   $ 1Q?69   5?      5?      ?? 5?      -   5        $ EmpIdReservation??5?   |   $??28??6   $
??Left=?6#   $??886,14,-6,0q?6g   $??1??6?   $	??Top=?6?   $	??188??6?    . {?.   6>   Z? ??5?      ?? 5?      $  0.5,1&?6*   $ 0.5,0&?6;   .??5?      ? $ White??6'   $ 0Q?65   ? 5?      $ 	#37FFFF00??6'   $ 1Q?69   5?      5?      ?? 5?      -   5        $ lblSerialId2??5?   ?   ???	#FF078200$??1q?6,   $??رقم الإيصال??6@   ???	#FF078200$??28??6m   $
??Left=?6y   $??Center=?6?   $??
630,44,0,0q?6?    . $	??Top=?6?   $	??120??6  ?? 5?      -    5        $	 	SerialId2??5?   x    . $??28??6/   $
??Left=?6;   $??
758,44,0,0q?6V   $ 5??6j   $??2??6?   $	??Top=?6?   $	??118??6?   {?.  .??6?   ??5?      ?? 5?      $  0.5,1&?6*   $ 0.5,0&?6;   .??5?      ? $ White??6'   $ 0Q?65   ? 5?      $ 	#37FFFF00??6'   $ 1Q?69   5?      .??5?   &   5?      ?? 5?      -!   5        $ Returned??5?   d   $??
مرتجع??6   $??28??6'   $
??Left=?63   $??292,406,0,42q?6N   $??2??6v   $??Bottom=?6?   $	??109??6?   ??.  6?     5?      -"   5        $ ReturnedDate??5?   p   $ Saturday=?6   $??28??63   $
??Left=?6?   $??417,406,0,42q?6Z   $??12??6?   $??Bottom=?6?   $	??130??6?   ?? 5?      -#   5        $ EmpIdReturned??5?   }   $??28??6   $
??Left=?6#   $??551,406,0,42q?6g   $??1??6?   $??Bottom=?6?   $	??188??6?    . {?.   6>   Z? ??5?      ?? 5?      $  0.5,1&?6*   $ 0.5,0&?6;   .??5?      ? $ White??6'   $ 0Q?65   ? 5?      $ 	#37FFFF00??6'   $ 1Q?69   5?      5?      ?? 5?      -$   5        $ 	CompanyId??5?   {   $??28??6   $
??Left=?6#   $??
832,77,0,0q?6g   $??2??6?   $	??Top=?6?   $	??187??6?   {?.   6>   Z? ??5?      ?? 5?      $  0.5,1&?6*   $ 0.5,0&?6;   .??5?      ? $ White??6'   $ 0Q?65   ? 5?      $ 	#37FFFF00??6'   $ 1Q?69   5?      5?      7  1System.Windows.Forms.Integration.WindowsFormsHost  5?      -%   5          Name$	 WFH??5?   7   $??292,111,10,136q?6      TabIndex$ 10??6Z      
Background 6D   Z? 6Z   ?? 5?      -&   5        $ lblLastEntry_Copy??5?   ?   ???	#FF078200$??1q?6,   $??الإجمالي??6@   ???	#FF078200$??28??6j   $??Right=?6v   $??Center=?6?   $??
0,0,136,78q?6?    . $??Bottom=?6?   $	??120??6  ?? 5?      -'   5        $	 lblTotal??5?   x    . $??28??6/   $??Right=?6;   $??	0,0,10,78q?6W   $ 5??6j   $??2??6?   $??Bottom=?6?   $	??118??6?   
 . .??6?   ??5?      ?? 5?      $  0.5,1&?6*   $ 0.5,0&?6;   .??5?      ? $ White??6'   $ 0Q?65   ? 5?      $ 	#37FFFF00??6'   $ 1Q?69   5?      .??5?   &   5?      ?? 5?      -(   5        $	 CurrentShift??5?   y    . $??28??6/   $??Right=?6;   $??
0,0,10,111q?6W   $ 5??6k   $??2??6?   $??Bottom=?6?   $	??118??6?   {?.  .??6?   ??5?      ?? 5?      $  0.5,1&?6*   $ 0.5,0&?6;   .??5?      ? $ White??6'   $ 0Q?65   ? 5?      $ 	#37FFFF00??6'   $ 1Q?69   5?      .??5?   &   5?      ?? 5?      -)   5        $	 InsertedDate??5?   y    . $??28??6/   $??Right=?6;   $??
0,0,10,144q?6W   $ 5??6k   $??2??6?   $??Bottom=?6?   $	??118??6?   {?.  .??6?   ??5?      ?? 5?      $  0.5,1&?6*   $ 0.5,0&?6;   .??5?      ? $ White??6'   $ 0Q?65   ? 5?      $ 	#37FFFF00??6'   $ 1Q?69   5?      .??5?   &   5?      5?      ?? 5?   
   -*   5        $??btnFirst??5?   T   $??أول??6   $??Center=?6    $??186,0,420,12q?6>   $??16??6d   $??Bottom=?6r   $??72??6?    
 BgSave#???  6?   ?? 5?   
   -+   5        $??
btnPrevios??5?   U   $??سابق??6   $??Center=?6!   $??264,0,342,12q?6?   $??17??6g   $??Bottom=?6u   $??72??6?   #???  6?   ?? 5?   
   -,   5        $??btnNext??5?   U   $??تالى??6   $??Center=?6!   $??342,0,264,12q?6?   $??18??6d   $??Bottom=?6r   $??72??6?   #???  6?   ?? 5?   
   --   5        $??btnLast??5?   U   $??أخير??6   $??Center=?6!   $??420,0,186,12q?6?   $??19??6d   $??Bottom=?6r   $??72??6?   #???  6?   ?? 5?   
   -.   5        $??btnSave??5?   9   $??
طباعة??6   $??186,0,342,58q?6#   $??13??6H   $??Bottom=?6V   $??Center=?6q   $	??150??6?   #???  6?   ?? 5?   
   -/   5        $??btnNew??5?   U   $??جديد??6   $??Center=?6!   $??627,0,394,58q?6?   $??14??6c   $??Bottom=?6q   $??71??6?    	 BgNew#???  6?   ?? 5?   
   -0   5        $??	btnDelete??5?   7   $??مسح??6   $??714,0,306,54q?6!   $??15??6H   $??Bottom=?6V   $??Center=?6q   $??72??6?     BgDelete#???  6?   $y?Hidden=?6?   ?? 5?   
   -1   5         ??Name$ btnSaveWithoutPrint??5?   7   $??حفظ??6   $??548,0,471,58q?6!   $??13??6T   $??Bottom=?6b   $??Center=?6}   $??73??6?   #???  6?   ??.  6?   ?? 5?   
   -2   5        $ btnDeleteRow??5?   W   $??حذف سطر??6   $??Center=?6#   $??818,0,196,54q?6A   $	??103??6m   $??Bottom=?6|   $??78??6?   #???  6?   $??38??6?   ??.  6?   5?      5?      