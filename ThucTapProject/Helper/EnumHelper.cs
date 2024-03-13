namespace ThucTapProject.Helper {
    enum Roles {
        admin=1,
        employee,
        clien
    }
    enum Account_status //Enum_entity_account
    {
        active = 1,
        inactive,
        blocked,
        validated,
        notValidated,
        removed
    }
    enum Product_status {
        InStock = 1, // còn hàng
        BackOrder, // Tạm thời hết hạn và sẽ được chuyển hàng vào kho sớm
        Discontinued, // ngừng kinh doanh
        OutOfStock // hết hàng dài hạn
    }

    enum ProducImage_status {
        Invalid,
        Valid
    }

    enum Order_status {
        Processing = 1,
        Preparing,
        Shipped,
        Delivered
        //Canceled
    }

    enum Payment_methods {
        Credit_card = 1,
        Debit_card = 3,
        Cash = 4
    }
    enum Payment_status {
        paying,
        success,
        fail
    }

    enum Simple_status {
        Invalid,
        Valid
    }
    class EnumHelper {
        public static string Log(Account_status status) {
            return "";
        }

        /*  public static string Log(Enum_entity_payment status)
          {
              return "";
          }*/
    }
}
