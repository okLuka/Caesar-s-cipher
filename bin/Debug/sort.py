#encoding=UTF-8
f = open("en.txt", encoding='UTF-8')
arr = []

for i in f.read().split('\n'):
    arr.append(i.lower()+'\n')

arr.sort()

new_file = open("en1.txt", 'w', encoding='utf-8')
new_file.writelines(arr)
new_file.close()