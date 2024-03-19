local M = {}

local function my_print()
	local win = vim.api.nvim_get_current_win()
	local width = math.floor(vim.api.nvim_win_get_width(win) / 2)
	local height = math.floor(vim.api.nvim_win_get_height(win))
	local help_buffer = vim.api.nvim_create_buf(false, true)
	local new_buffer = vim.api.nvim_create_buf(false, true)
	vim.api.nvim_open_win(help_buffer, false,
		{ relative = 'win', width = width, height = 20, bufpos = { 0, 0 }, border = 'single' })
	vim.api.nvim_open_win(new_buffer, true,
		{ relative = 'win', width = width, height = height - 21, bufpos = { 20, 0 }, border = 'single' })
end

function M.setup(config)
	vim.api.nvim_create_user_command("MyPrint", my_print, {})
end

return M
