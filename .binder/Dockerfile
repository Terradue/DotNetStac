FROM terradue/dotnetinteractive:latest

USER root

# Copy project source
COPY ./src/ ${HOME}/src/
# Copy notebooks
COPY ./notebooks/ ${HOME}/notebooks/

# Copy package sources
COPY ./.binder/NuGet.config ${HOME}/nuget.config

RUN chown -R ${NB_UID} ${HOME}
USER ${USER}

RUN dotnet build ${HOME}/src/

# Set root to notebooks
WORKDIR ${HOME}/notebooks/