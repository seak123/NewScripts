local this = class("entire_skill")

function this:ctor( root )
    self.root = root
end

function this:execute( database )
    self.root.childs[1]:update(nil,database)
end

return this