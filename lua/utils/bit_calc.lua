local this = {data32={}}

for i=1,32 do
    this.data32[i]=2^(32-i)
end

function this.d2b(arg)
    local   tr={}
    for i=1,32 do
        if arg >= this.data32[i] then
        tr[i]=1
        arg=arg-this.data32[i]
        else
        tr[i]=0
        end
    end
    return   tr
end   --bit:d2b
 
function this.b2d(arg)
    local   nr=0
    for i=1,32 do
        if arg[i] ==1 then
        nr=nr+2^(32-i)
        end
    end
    return  nr
end   --bit:b2d

function this._and( a,b )
    local   op1=this.d2b(a)
    local   op2=this.d2b(b)
    local   r={}
    
    for i=1,32 do
        if op1[i]==1 and op2[i]==1  then
            r[i]=1
        else
            r[i]=0
        end
    end
    return  this.b2d(r)
end

return this