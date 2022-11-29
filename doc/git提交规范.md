git提交规范
遇到的问题
1,版本回退时无法快速定位到指定版本
2,无法知道项目中封版操作是哪一个commit
3,无法清晰的知道每次提交的记录
以下是commit提交规范
每次提交，Commit message 都包括三个部分：Header，Body 和 Footer。
其中，Header 是必需的，Body 和 Footer 可以省略。

<type>(<scope>): <subject>
// 空一行
<body>
// 空一行

<footer>
1
2
3
4
5
Header
Header部分只有一行，包括两个字段：type（必需）和subject（必需）。
type用于说明 commit 的类别，只允许使用下面9个标识。
feat: 新功能（feature）
fix: 修补bug
docs: 文档（documentation）
style: 格式（不影响代码运行的变动）
refactor: 重构（即不是新增功能，也不是修改bug的代码变动）
chore: 构建过程或辅助工具的变动
revert: 撤销，版本回退
perf: 性能优化
test：测试
improvement: 改进
build: 打包
ci: 持续集成
1
2
3
4
5
6
7
8
9
10
11
12
subject是 commit 目的的简短描述，不超过50个字符。

以动词开头，使用第一人称现在时，比如修改/修复/增加 等等
example:修复xxx功能
1
2
scope
scope用于说明 commit 影响的范围，比如数据层、控制层、视图层等等，视项目不同而不同。

Body
Body 部分是对本次 commit 的详细描述，可以分成多行。下面是一个范例。

xxx/xxx.vue  修改内容
xxx/xxx.vue  修改内容
1
2
文档综合了，公司项目git管理的实际情况，在某一些规范上进行了轻微修改，在平时提交fix和feat时，一般使用简写

feat: 增加订单详情  closes xxxx (closes非必需)
fix: 修复xx情况下xx问题  closes xxxx (closes非必需)
docs: 修改md文件
style: 修改订单列表样式
refactor: 重构utils.js下部分方法
chore: 增加xxx插件/xxxxloader 为了解决xx问题， 引入公用组件库
revert: 回退当前版本667ec到 sssee2
perf: 优化了xxx，提高了渲染速度
test：增加测试
improvement: 改进
build: 打包
ci: 持续集成