FROM terradue/dotnetinteractive:latest

USER root

# Copy package sources
COPY ./.binder/NuGet.config ${HOME}/nuget.config

# Copy project source
COPY ./src/ ${HOME}/src/
# Copy notebooks
COPY ./notebooks/ ${HOME}/notebooks/

RUN chown -R ${NB_UID} ${HOME}
USER ${USER}

RUN dotnet publish ${HOME}/src/ -f netstandard2.0

# Set root to notebooks
WORKDIR ${HOME}/notebooks/