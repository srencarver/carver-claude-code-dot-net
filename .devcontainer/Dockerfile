# Contenedor de desarrollo (sesión 4): SDK de .NET 8, Git y Claude Code, con el usuario app (no root).
# Claude Code no admite --dangerously-skip-permissions como root.
FROM mcr.microsoft.com/dotnet/sdk:8.0
# El hook de .claude/settings.json llama a "powershell"; en Linux la imagen trae pwsh.
RUN ln -s /usr/bin/pwsh /usr/local/bin/powershell
USER app
# El repositorio se monta con otro propietario: sin esto Git lo rechaza (dubious ownership).
RUN git config --global --add safe.directory /workspace
# El directorio de configuración se crea aquí para que el volumen herede el propietario app.
RUN curl -fsSL https://claude.ai/install.sh | bash \
 && mkdir -p /home/app/.claude-config
ENV PATH="/home/app/.local/bin:${PATH}"
WORKDIR /workspace
