
require 'clasp'
require 'recls'

Arguments	=	Clasp::Arguments::new(ARGV)
Flags		=	Arguments.flags
Options		=	Arguments.options
Values		=	Arguments.values.to_a

keyFile = Values.shift

abort "USAGE: #$0 <key-file>" if not keyFile

abort "key file '#{keyFile}' does not exist" if not File::file? keyFile

Recls::FileSearch::new('.', '*.dll|*.exe', Recls::RECURSIVE).each do |fe|

	puts "processing #{fe.searchRelativePath}:"

	system("sn.exe -q -R #{fe.path} #{keyFile}")
	system("sn.exe -q -Vr #{fe.file}") # this is only done to avoid warnings from next command (when not previously ignored)
	system("sn.exe -q -Vu #{fe.file}")
end

