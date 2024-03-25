local M = {}

local client = vim.lsp.start_client({
	name = "my-lsp",
	cmd = { "my-lsp" },
})

if not client then
	vim.notify("Failed to start my-lsp", vim.log.levels.ERROR)
	return
end


vim.api.nvim_create_autocmd("FileType", {
	pattern = "markdown",
	callback = function()
		vim.lsp.buf_attach_client(0, client)
	end
})

function M.setup(config)
	print("Setting up my-lsp", config)
end

return M
