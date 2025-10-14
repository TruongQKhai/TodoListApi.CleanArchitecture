create trigger PreventDeleteTodoList
on TodoLists
instead of delete
as 
begin 
	print 'không thể xóa TodoList đã có TodoItem';
end