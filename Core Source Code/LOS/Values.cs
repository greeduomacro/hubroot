//--------------------------------------------------------------------------------
// Copyright Joe Kraska, 2006. This file is restricted according to the GPL.
// Terms and conditions can be found in COPYING.txt.
//--------------------------------------------------------------------------------
using System;
using System.Reflection;
using System.IO;
using System.Text;
//--------------------------------------------------------------------------------
namespace Server.LOS {
//--------------------------------------------------------------------------------
//  Values class; encompasses static types found only on the scripts side of thing
//                as well as various constants.
//--------------------------------------------------------------------------------
public class Values
{
      public static readonly Type Corpse = ScriptCompiler.FindTypeByFullName("Server.Items.Corpse");
}
//--------------------------------------------------------------------------------
}
//--------------------------------------------------------------------------------
