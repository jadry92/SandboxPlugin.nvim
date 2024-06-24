local M = {}

local base_path = os.getenv("PROJECTS_DIR")

local client = vim.lsp.start_client({
	name = "MarkdownLSP",
	cmd = { base_path .. "/MarkdownLSP/exe/LSP" },
	on_attach = function(_, bufnr)
		local nmap = function(keys, func, desc)
			if desc then
				desc = 'LSP: ' .. desc
			end

			vim.keymap.set('n', keys, func, { buffer = bufnr, desc = desc })
		end

		nmap('<leader>rn', vim.lsp.buf.rename, '[R]e[n]ame')
		nmap('<leader>ca', vim.lsp.buf.code_action, '[C]ode [A]ction')

		nmap('gd', require('telescope.builtin').lsp_definitions, '[G]oto [D]efinition')
		nmap('gr', require('telescope.builtin').lsp_references, '[G]oto [R]eferences')
		nmap('gI', require('telescope.builtin').lsp_implementations, '[G]oto [I]mplementation')
		nmap('<leader>D', require('telescope.builtin').lsp_type_definitions, 'Type [D]efinition')
		nmap('<leader>ds', require('telescope.builtin').lsp_document_symbols, '[D]ocument [S]ymbols')
		nmap('<leader>ws', require('telescope.builtin').lsp_dynamic_workspace_symbols, '[W]orkspace [S]ymbols')

		-- See `:help K` for why this keymap
		nmap('K', vim.lsp.buf.hover, 'Hover Documentation')
	end
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
