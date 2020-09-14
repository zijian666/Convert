项目代码已经移到[码云](https://gitee.com/zijian666/super-convert)

# Converts

超级转换器 
> .NET Standard 2.0 重新设计  
> *老项目已经停止维护 [Convert3](https://github.com/blqw/blqw.Convert3)*

# 说明
> 对象转换，从未如此简单  
```csharp
obj.To<T>();                //转换失败,抛出异常
obj.To<T>(T defaultValue);  //转换失败,返回默认值
obj.To<T>(out succeed);     //输出转换是否成功
//下面3个是非泛型方法 
obj.To(Type outputType);
obj.To(Type outputType, object defaultValue);
obj.To(Type outputType, out succeed);
```

## 代码展示
```csharp
//最基本
"1".To<int>();
"a".To<int>(0); //转换失败返回 0
"是".To<bool>(); //支持 "是/否" "真/假" "对/错" "t/f" "true/false" 等
byte[].To<Guid>();

//进阶
"1,2,3,4,5,6".To<int[]>();
"{\"id\":\"name\":\"blqw\"}".To<User>();
Dictionary.To<Entity>(); //键值对转实体
DataRow.To<Entity>(); //数据行转实体
DataTable.To<List<Entity>>; //数据表转实体集合

//更复杂
DataTable.To<List<NameValueCollection>>(); 
List<Dictionary<string, object>>.To<DataTable>(); 
new { ID=1, Name="blqw"}.To<User>(); //匿名类转换

//变态嵌套
Dictionary<Guid, Dictionary<int, User>>
    .To<Dictionary<string, Dictionary<DateTime, NameValueCollection>>>(); //不能理解就算了
```
## 扩展自定义转换器
```csharp
[TestMethod]
public void 自定义转换器()
{
    var i = 11;
    var my = i.To<MyClass>();
    Assert.AreEqual(i, my.ID);
}

public class MyClass
{
    public int ID { get; set; }
}

public class MyConvertor : AllowNullConvertor<MyClass>, IFrom<int, MyClass>
{
    public ConvertResult<MyClass> From(IConvertContext context, int input)
    {
        return new MyClass() { ID = input };
    }
}
```

## 智能识别自定义转换方法
```csharp
[TestMethod]
public void 自定义强转()
{
    var i = 11;
    var my = i.To<MyClass>();
    Assert.AreEqual(i, my.ID);
}

public class MyClass
{
    public int ID { get; set; }
    public static explicit operator MyClass(int i) => new MyClass() { ID = i };
}
```
```csharp
[TestMethod]
public void 自定义隐转()
{
    var i = 11;
    var my = i.To<MyClass2>();
    Assert.AreEqual(i, my.ID);
}

public class MyClass2
{
    public int ID { get; set; }
    public static implicit operator MyClass2(int i) => new MyClass2() { ID = i };
}
```

## 其他功能
```csharp
//数字转大写
Console.WriteLine(Converts.ToChineseAmount("123456456.789")); //壹亿贰仟叁佰肆拾伍万陆仟肆佰伍拾陆元柒角捌分
Console.WriteLine(Converts.ToChineseNumber("123456456.789")); //一亿二千三百四十五万六千四百五十六点七八九
Console.WriteLine(Converts.ToChineseAmount("123456456.789", true)); //一亿二千三百四十五万六千四百五十六元七角八分

//全半角转换
Console.WriteLine(Converts.ToDBC("，１２３４５６７ａｋｓ"));//,1234567aks
Console.WriteLine(Converts.ToSBC("!1f23d.?@"));         //！１ｆ２３ｄ．？＠

//摘要/加密
Console.WriteLine(Converts.ToMD5("123456"));    //e10adc3949ba59abbe56e057f20f883e
Console.WriteLine(Converts.ToSHA1("123456"));   //7c4a8d09ca3762af61e59520943dc26494f8941b

//随机加密
var arr = new[]
{
    Converts.ToRandomMD5("123456"),
    Converts.ToRandomMD5("123456"),
    Converts.ToRandomMD5("123456"),
    Converts.ToRandomMD5("123456"),
    Converts.ToRandomMD5("123456"),
};

foreach (var g in arr)
{
    Console.WriteLine($"{g} : {Converts.EqualsRandomMD5("123456", g)}");
}
/*
fa91eefc-e903-dbcf-394b-0b757987357b : True
27abd3e0-fe0e-2eeb-1ff7-a60b03876465 : True
6d911bf2-0c59-0e01-5e87-7527dd1ee699 : True
0af7905a-0b3b-4eb4-b82b-0340f3438924 : True
1e024253-6bb9-fb25-4b67-3e42c265af02 : True
*/
```


## 更新说明 
### [4.0.0.4-beta]2020.09.09
+ 动态加载Json支持组件(nuget安装zijian666.SuperConvert.Json)
* 优化部分数据转换逻辑

### [4.0.0.3-beta]2020.09.07
+ 依赖核心包

### [4.0.0-beta]2020.09.06
* 升级到standard2.0
