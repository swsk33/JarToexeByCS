# C Sharp实现jar转exe程序模板
#### 介绍
一个简单的使用C#代码，实现jar打包成exe的程序模板。支持32位和64位操作系统。<br>
#### 使用说明
现将系统自带的C#编译器(csc.exe)所在目录添加到Path系统环境变量里面：<br>
此电脑-右键-属性-高级系统设置-环境变量，在系统变量里打开Path变量，把路径```C:\Windows\Microsoft.NET\Framework\v4.0.30319```加进去。然后打开命令行即可调用```csc```命令。<br>
然后把src文件夹里面的Program.cs源文件复制出来，把要打包的jar文件命名为mainJar.jar放在和Program.cs同路径下。<br>
使用命令行的cd命令进入到Program.cs和mainJar.jar所在目录，输入命令进行编译，命令格式如下：<br>
```
csc /t:winexe /res:mainJar.jar /platform:输出架构 /win32icon:指定ico图标文件路径作为程序图标 /out:输出exe文件路径 Program.cs
```
上述/win32icon和/platform两个参数需自己指定，并且是非必须的；/out自己指定，其余不能改变。<br>
/platform可选参数如下：<br>
```
anycpu：可在任何架构的cpu上运行，若不带/platform参数则默认输出anycpu的程序
x86：32位程序
x64：64位程序
arm：arm架构
```
例如：
```
//打包jar为可在任何cpu架构运行的exe，并指定当前目录下icon.ico文件作为exe图标，输出exe到当前目录下Main.exe
csc /t:winexe /res:mainJar.jar /platform:anycpu /win32icon:icon.ico /out:Main.exe Program.cs
```
部分配置可以用文本编辑器打开Program.cs进行修改，例如程序类型（控制台或者窗口）、java路径、找不到java运行环境的提示信息等等，其中在源文件中都有注释说明“可修改”。<br>
> 最后更新：2021.2.8