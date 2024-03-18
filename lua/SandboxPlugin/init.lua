local M = {}

function my_print()
	vim.api.nvim_open_win(0, false,
		{ relative = 'win', width = 12, height = 3, bufpos = { 100, 10 } })
end

function M.setup(config)
	vim.api.nvim_create_user_command("MyPrint", my_print, {})
end

return M
