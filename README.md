# C Sharp实现jar转exe程序模板
### 介绍
一个简单的使用C#代码，实现jar打包成exe的程序模板。支持32位和64位操作系统。<br>
### 使用说明
#### 1，环境配置
先将系统自带的C#编译器(csc.exe)所在目录添加到Path系统环境变量里面：<br>
此电脑-右键-属性-高级系统设置-环境变量，在系统变量里打开Path变量，把路径```C:\Windows\Microsoft.NET\Framework\v4.0.30319```加进去。然后打开命令行/cmd输入```csc```命令，如果有输出说明配置成功。<br>
#### 2，下载发行版并解压
[下载](https://gitee.com/swsk33/jarToexeByCS/releases)右边发行版/Release中的"j2ecs-x.x.x.7z"（x表示版本号，下载最新版即可）并解压。<br>
#### 3，按需修改配置文件
在解压的文件夹中有一个"cfg.properties"文件，可以使用文本编辑器打开，这是全局配置文件，没有特殊需要可以不修改，不过大多数时候可能需要修改，里面配置值代表如下：<br>
- isConsole=是否是控制台应用程序，填入true为是false为否
- isPause=为控制台程序时是否在最后暂停，填入true为是false为否
- writeErrorToLog=是否是把错误输出写入文件，true为是false为否
- logFileLocation=若把错误输出写入文件，自定义错误输出位置
- javaPath=java的路径，在已经安装并配置了java环境变量时直接填java即可，否则就要指定其路径
- errorMsg=未找到java运行环境时的错误提示
- preArgs=预先参数，会先于传入参数执行

配置文件中以#开头的一行是注释，构建exe时不会读取注释内容，默认情况下配置文件是全部被注释的状态。可根据自己需要去掉配置值的注释并填入配置值。<br>
#### 4，打开命令行/cmd调用buildexe进行jar到exe的构建
使用命令行/cmd的cd命令进入到文件夹，输入命令调用buildexe.exe文件，命令形式如下：<br>
```
buildexe -j jar文件路径 -o 输出exe路径 [-p 架构] [-i ico图标文件路径] [-a]
```
上述命令中中括号括起来部分是可选参数，实际加上这些可选参数执行时不需要写中括号。<br>
架构(-p)参数可选值如下：<br>
anycpu --- 可在任何架构的cpu上运行（默认）<br>
x86 --- 32位程序<br>
x64 --- 64位程序<br>
arm --- arm架构<br>
-a表示该程序是否需要管理员权限，不带-a即为不需要管理员权限。<br>
上述参数顺序可以任意写。<br>
#### 5，实例
这里给几个例子。<br>
**将E:\\中转\\a.jar打包为main.exe放到用户下载文件夹：**<br>
```
buildexe -j "E:\中转\a.jar" -o "C:\Users\%username%\Downloads\main.exe"
```
**将E:\\中转\\a.jar打包为main.exe放到用户下载文件夹，并指定exe图标为C:\\icon\\ex.ico：**<br>
```
buildexe -j "E:\中转\a.jar" -o "C:\Users\%username%\Downloads\main.exe" -i "C:\icon\ex.ico"
```
**将E:\\中转\\a.jar打包为main.exe放到用户下载文件夹并使其以管理员身份运行：**<br>
```
buildexe -j "E:\中转\a.jar" -o "C:\Users\%username%\Downloads\main.exe" -a
```
> 最后更新：2021.6.17