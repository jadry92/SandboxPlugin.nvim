local M = {}

local base_path = os.getenv("PROJECTS_DIR")

local client = vim.lsp.start_client({
	name = "MarkdownLSP",
	cmd = { base_path .. "/MarkdownLSP/exe/LSP" },
})

if not client then
	vim.notify("Failed to start my-lsp", vim.log.levels.ERROR)
	return
end


vim.api.nvim_create_autocmd("FileType", {
	pattern = "markdown",
	callback = function()
		local status = vim.lsp.buf_attach_client(0, client)
		if not status then
			vim.notify("Failed to attach my-lsp to buffer", vim.log.levels.ERROR)
		end
		print("Attached my-lsp to buffer")
	end,
})

function M.setup(config)
	print("Setting up my-lsp")
end

return M
