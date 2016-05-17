# LiteHttpUtil
a simple HTTP request util .
self-used, just for a specific project.  
自用！

##发起HTTP请求
封装了常见的4种HTTP请求：GET POST PUT DELETE
每种请求又封装了直接返回请求值（string）的同步方法，和一个返回HttpResponseMessage任务的异步方法。
对于可以在消息体（body）中传递参数的POST和PUT请求来讲，支持将消息体中的参数以JSON格式字符串的形式或者以标准的Form键值对格式作为参数进行调用。

**GET请求**
`string Get(url, [timeOut]);`
`Task<HttpResponseMessage> GetAsync(httpClient, url, [timeOut]);`

**POST请求**
`string Post(url, postKvps, [timeOut]);`
`string Post(url, postJson, [timeOut]);`
`Task<HttpResponseMessage> PostAsync(httpClient, url, postKvps, [timeOut]);`
`Task<HttpResponseMessage> PostAsync(httpClient, url, postJson, [timeOut]);`

**PUT**
`string Put(url, postKvps, [timeOut]);`
`string Put(url, postJson, [timeOut]);`
`Task<HttpResponseMessage> PutAsync(httpClient, url, postKvps, [timeOut]);`
`Task<HttpResponseMessage> PutAsync(httpClient, url, postJson, [timeOut]);`

**DELETE**
`string Delete(url, [timeOut]);`
`Task<HttpResponseMessage> DeleteAsync(httpClient, url, [timeOut]);`

##HTTP请求通道控制
对于某些特定的请求，要对使用频率(暂缺)或者失败率做发送通道控制的，需先实例化一个HttpEntrance对象：
`new HttpEntrance(id, 关闭持续时长, 关闭触发条件)`

*目前关闭触发条件仅支持失败次数，连续发送失败达到设置值的时候将关闭请求通道*

###Properties
**ID** ...
**LastCloseDtm** 上次关闭时间
**CloseDuration** 通道关闭持续时长
**CloseTriggerCount** 触发通道关闭的请求失败临界值
**requestRsList** 请求结果集


###Methods

**Switch([isOpen])**
这是HttpEntrance的最高开关。isOpen如果为true表示开启控制（实例化后默认为true）；如果为false将关闭控制，关闭控制后，将不再限制http请求的发送，各类查询方法也均返回正常；如果不传递isOpen参数，将返回当前switch的值（调用方法类似于jQuery）;


**IsOpen([ifClosedAction])**
查看当前http通道是开启状态还是关闭状态，ifClosedAction的Type为Action，将仅在通道关闭的情况下调用。

**AddResult(trueOrFalse, [isTryClose])**
添加一次请求成功与否的结果（暂不支持调用HttpReqUtil的请求方法后自动判断本次请求是否成功）。参数isTryClose选填，表示如果当前结果录入后已经达到可以关闭HTTP请求通道的标准，是否立即关闭，默认值为true。

**ChangeCloseTriggerCoun**
修改请求触发通道关闭的条件值。

**ResetClosedLastTime**
修改通道关闭持续时长。

**GetClosedRemaining**
获取距离通道自动开启还剩多少时间，如果当前通道本来就是开启状态，返回0。

**GetReqFailedRate**
获取请求失败率。若请求失败次数小于设定的通道关闭触发临界值，返回的直接为0。

**ResetAll**
重置所有参数成初始化时的值。

**Open**
开启HTTP请求通道。

**ReOpen**
重新开启HTTP请求通道，并清除之前的请求结果历史记录。

**Close**
关闭HTTP请求通道。