MyModernMovieManager
=====================

## 开发说明
本软件是小学期期间为了完成软件课程设计所做。

前端使用一个基于WPF的Modern UI设计。核心是使用C#语言。自己实现基本的MVC设计架构。
其中数据存储是使用Nhibernate的ORM模型。大多数的模块使用的是单例模式。数据库使用的是MySQL。

## 开发环境
基于.Net Framework 4.5，visual studio 2012 update 3开发，Nhibernate 3.3。其中有一个模块是
使用ffmpeg实现，所以在生成的可执行程序的文件夹需放入ffmpeg.exe。数据库为MySQL 5.5。

## 模块说明
### Tools
#### Fish.MovieManager.Test
开发过程中的测试类
#### Fish.MovieManager.GetFile
一些辅助函数

### Model
#### Fish.MovieManager.DoubanAPI
简单的封装了几个从豆瓣上抓取数据的API

#### Fish.MovieManager.DoubanActorInfo
数据库中DoubanActorInfo表的结构和Nhibernate的映射

#### Fish.MovieManager.DoubanMovieInfo
数据库中DoubanMovieInfo表的结构和Nhibernate的映射

#### Fish.MovieManager.VideoFileInfo
数据库中VideoFileInfo表的结构和Nhibernate的映射。VideoEcoderAsync中封装了
利用ffmpeg从视频文件获取相关信息。

#### Fish.MovieManager.Movie2Actor
数据库中Movie2Actor表的结构和Nhibernate的映射

#### Fish.MovieManager.Movie2Tag
数据库中Movie2Tag表的结构和Nhibernate的映射

## 许可协议
Copyright [2013] [maemual]

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.