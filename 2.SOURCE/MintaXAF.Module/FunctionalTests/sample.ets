;#DropDB MintaXAFEasyTest

#Application MintaXAF.Win
#Application MintaXAF.Web

*FillForm
 User Name = Admin
*Action Log In

#IfDef MintaXAF.Win

*Action About...
 
*Action OK

#EndIf

#IfDef MintaXAF.Web
*Action Navigation(My Details)
#EndIf
