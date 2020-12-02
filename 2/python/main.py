def validates_part_one(password, letter, min, max):
    return min <= password.count(letter) <= max

def validates_part_two(password, letter, first_position, second_position):
    return (password[first_position - 1] == letter) ^ (password[second_position - 1] == letter)

with open("..\input.txt") as _file:
    lines = [line.strip() for line in _file]

    part1count = 0
    part2count = 0
    for i, line in enumerate(lines):
        tokens = line.split()
        password = tokens[2]
        letter = tokens[1].replace(':','')
        first_num = int(tokens[0].split('-')[0])
        second_num = int(tokens[0].split('-')[1])
        
        if(validates_part_one(password, letter, first_num, second_num)):
            part1count = part1count + 1

        if(validates_part_two(password, letter, first_num, second_num)):
            part2count = part2count + 1

    print("Part 1: ", part1count)
    print("Part 2: ", part2count)

# Output:
# Part 1:  645
# Part 2:  737