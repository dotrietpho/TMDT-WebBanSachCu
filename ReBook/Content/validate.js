function checkoutValidate(addr, phoneNum) {
    if (addr && phoneNum && phoneNum.length > 8) {
        return true;
    }
    return false;
}