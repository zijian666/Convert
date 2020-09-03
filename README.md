# Converts
����ת���� 
> *�������, ����Ŀ�Ѿ�ֹͣά�� [Convert3](https://github.com/blqw/blqw.Convert3)*

# ˵��
> ����ת������δ��˼�  
```csharp
obj.To<T>();                //ת��ʧ��,�׳��쳣
obj.To<T>(T defaultValue);  //ת��ʧ��,����Ĭ��ֵ
obj.To<T>(out succeed);     //���ת���Ƿ�ɹ�
//����3���ǷǷ��ͷ��� 
obj.To(Type outputType);
obj.To(Type outputType, object defaultValue);
obj.To(Type outputType, out succeed);
```

## ����չʾ
����ʾ������: [demo](https://github.com/blqw/blqw.Converts/blob/master/Demo/Program.cs) , [��Ԫ����1](https://github.com/blqw/blqw.Converts/blob/master/UnitTest/%E5%9F%BA%E6%9C%AC%E5%8A%9F%E8%83%BD%E6%B5%8B%E8%AF%95.cs)  
```csharp
//�����
"1".To<int>();
"a".To<int>(0); //ת��ʧ�ܷ��� 0
"��".To<bool>(); //֧�� "��/��" "��/��" "��/��" "t/f" "true/false" ��
byte[].To<Guid>();

//����
"1,2,3,4,5,6".To<int[]>();
"{\"id\":\"name\":\"blqw\"}".To<User>();
Dictionary.To<Entity>(); //��ֵ��תʵ��
DataRow.To<Entity>(); //������תʵ��
DataTable.To<List<Entity>>; //���ݱ�תʵ�弯��

//������
DataTable.To<List<NameValueCollection>>(); 
List<Dictionary<string, object>>.To<DataTable>(); 
new { ID=1, Name="blqw"}.To<User>(); //������ת��

//��̬Ƕ��
Dictionary<Guid, Dictionary<int, User>>
    .To<Dictionary<string, Dictionary<DateTime, NameValueCollection>>>(); //������������
```
## ��չ�Զ���ת����
> ��չת����Ϊ����**Ŀ������**Ϊ���յ�  
> ����Ҫת��Ϊ�Զ�������`MyClass`����Ҫ**ʵ��`IConvertor<MyClass>`**�ӿ�  
> ����ֱ��**�̳л��� `BaseConvertor<MyClass>`**,������Ĭ��**�޲ι��캯��**  
> IOC������Զ�װ��ʵ��`IConvertor`��ȫ������

#### 1. `MyClass`����
```csharp
class MyClass
{
    public int Number { get; set; }
}
```
#### 2. ת��������
```csharp
class MyClassConvertor : BaseConvertor<MyClass>
{
    protected override MyClass ChangeType(ConvertContext context, object input, Type outputType, out bool success)
    {
        var i = context.Get<int>().ChangeType(context, input, typeof(int), out success);
        return success ? new MyClass() { Number = i } : null;
    }

    protected override MyClass ChangeType(ConvertContext context, string input, Type outputType, out bool success)
    {
        var i = context.Get<int>().ChangeType(context, input, typeof(int), out success);
        return success ? new MyClass() { Number = i } : null;
    }
}
```
#### 3. ���Դ���
```csharp
var x = "1234".To<MyClass>(null);
Console.WriteLine(x?.Number); //1234
x = "abcd".To<MyClass>(null);
Console.WriteLine(x?.Number); //null
```
## ��������
```csharp
//����ת��д
Console.WriteLine(Converts.ToChineseAmount("123456456.789")); //Ҽ�ڷ�Ǫ������ʰ����½Ǫ������ʰ½Ԫ��ǰƷ�
Console.WriteLine(Converts.ToChineseNumber("123456456.789")); //һ�ڶ�ǧ������ʮ������ǧ�İ���ʮ�����߰˾�
Console.WriteLine(Converts.ToChineseAmount("123456456.789", true)); //һ�ڶ�ǧ������ʮ������ǧ�İ���ʮ��Ԫ�߽ǰ˷�

//ȫ���ת��
Console.WriteLine(Converts.ToDBC("��������������������"));//,1234567aks
Console.WriteLine(Converts.ToSBC("!1f23d.?@"));         //�����棲���䣮����

//ժҪ/����
Console.WriteLine(Converts.ToMD5("123456"));    //e10adc3949ba59abbe56e057f20f883e
Console.WriteLine(Converts.ToSHA1("123456"));   //7c4a8d09ca3762af61e59520943dc26494f8941b

//�������
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


## ����˵�� 
* �������, �����ڴ�