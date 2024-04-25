@echo off

: for every arg, eg trees_bark_a_01, this script will 
: copy the matching dds texture, and bump map if any, to the destination 

set source=D:\X-Ray_CoP_SDK\editors\gamedata\textures
set destination=D:\CoP_textures

pushd %source%

for %%t in (%*) do (
    for /f %%G in ('dir /b/s %%t*.dds') do (copy %%G %destination% | echo copied %%G)
)

popd